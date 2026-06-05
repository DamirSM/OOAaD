using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;
using SpaceBattle.Tests;
using System.Collections.Concurrent;
using Xunit;
using Moq;

namespace SpaceBattle.Tests;

public class FireCommandTests : IDisposable
{
    private readonly object _previousScope;
    private readonly int _previousDenominator;
    
    public FireCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        _previousDenominator = Angle.Denominator; // сохраняем
        Angle.Denominator = 8;
        
        _previousScope = Ioc.Resolve<object>("Scopes.Current");
        var rootScope = Ioc.Resolve<object>("Scopes.Root");
        var newScope = Ioc.Resolve<object>("Scopes.New", rootScope);
        Ioc.Resolve<ICommand>("Scopes.Current.Set", newScope).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry", 
            (Func<object[], object>)(args => new Dictionary<Guid, IDictionary<string, object>>())
        ).Execute();
    }

    public void Dispose()
    {
        try
        {
            Ioc.Resolve<ICommand>("IoC.Register", "Actions.Start",
                (Func<object[], object>)(args => new StartCommand((IDictionary<string, object>)args[0], (string)args[1]))
            ).Execute();
        }
        catch { }
        try
        {
            var registry = Ioc.Resolve<Dictionary<Guid, IDictionary<string, object>>>("Game.Registry");
            registry?.Clear();
        }
        catch { }

        Angle.Denominator = _previousDenominator;
        
        try
        {
            Ioc.Resolve<ICommand>("Scopes.Current.Set", _previousScope).Execute();
        }
        catch { }
    }
    private void ClearRegistry()
    {
        try
        {
            var registry = Ioc.Resolve<ConcurrentDictionary<Guid, IDictionary<string, object>>>("Game.Registry");
            registry?.Clear();
        }
        catch { }
    }

    [Fact]
    public void ReceiveCallsExecuteOnCommand()
    {
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(c => c.Execute()).Verifiable();
        var receiver = new CommandReceiver();

        receiver.Receive(mockCommand.Object);

        mockCommand.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void FireCommandCreatesTorpedoAndStartsMove()
    {
        ClearRegistry();

        var ship = new Dictionary<string, object>();
        var shipPos = new Vector(0, 0);
        var shipAngle = new Angle(0, 360);

        Ioc.Resolve<ICommand>("IoC.Register", "Adapters.IMovingObject",
            (Func<object[], object>)(args => new MockMovingObject(shipPos))).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Adapters.IRotatingObject",
            (Func<object[], object>)(args => new MockRotatingObject(shipAngle))).Execute();

        var addedObjects = new Dictionary<Guid, IDictionary<string, object>>();
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry.Add",
            (Func<object[], object>)(args =>
            {
                var id = (Guid)args[0];
                var obj = (IDictionary<string, object>)args[1];
                addedObjects[id] = obj;
                return new EmptyCommand();
            })).Execute();

        bool startCalled = false;
        Ioc.Resolve<ICommand>("IoC.Register", "Actions.Start",
            (Func<object[], object>)(args =>
            {
                startCalled = true;
                Assert.Equal("Move", args[1]);
                return new EmptyCommand();
            })).Execute();

        var fireCommand = new FireCommand(ship);
        fireCommand.Execute();

        Assert.True(startCalled);
        Assert.Single(addedObjects);
        var torpedo = addedObjects.Values.First();
        Assert.Equal(shipPos, torpedo["Position"]);
        Assert.Equal(new Vector(1, 0), torpedo["Velocity"]);
        Assert.IsType<CommandReceiver>(torpedo["Receiver"]);
    }

    [Fact]
    public void FireCommandThrowsWhenShipAngleIsNull()
    {
        ClearRegistry();

        var ship = new Dictionary<string, object>();
        var shipPos = new Vector(0, 0);

        Ioc.Resolve<ICommand>("IoC.Register", "Adapters.IMovingObject",
            (Func<object[], object>)(args => new MockMovingObject(shipPos))).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Adapters.IRotatingObject",
            (Func<object[], object>)(args => new MockRotatingObject(null))).Execute();

        var fireCommand = new FireCommand(ship);
        Assert.Throws<InvalidOperationException>(() => fireCommand.Execute());
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(90, 0, 1)]
    [InlineData(180, -1, 0)]
    [InlineData(270, 0, -1)]
    public void FireCommandComputesVelocityDirection(int degrees, int expectedVx, int expectedVy)
    {
        ClearRegistry();

        var ship = new Dictionary<string, object>();
        var shipPos = new Vector(0, 0);
        var shipAngle = new Angle(degrees, 360);

        Ioc.Resolve<ICommand>("IoC.Register", "Adapters.IMovingObject",
            (Func<object[], object>)(args => new MockMovingObject(shipPos))).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Adapters.IRotatingObject",
            (Func<object[], object>)(args => new MockRotatingObject(shipAngle))).Execute();

        var addedObjects = new Dictionary<Guid, IDictionary<string, object>>();
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry.Add",
            (Func<object[], object>)(args =>
            {
                var id = (Guid)args[0];
                var obj = (IDictionary<string, object>)args[1];
                addedObjects[id] = obj;
                return new EmptyCommand();
            })).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Actions.Start",
            (Func<object[], object>)(args => new EmptyCommand())).Execute();

        var fireCommand = new FireCommand(ship);
        fireCommand.Execute();

        Assert.Single(addedObjects);
        var torpedo = addedObjects.Values.First();
        var velocity = (Vector)torpedo["Velocity"];
        Assert.Equal(new Vector(expectedVx, expectedVy), velocity);
    }

    [Fact]
    public void RegisterIoCDependencyFireCommandRegistersAndCreatesFireCommand()
    {
        var registerCommand = new RegisterIoCDependencyFireCommand();
        registerCommand.Execute();

        var ship = new Dictionary<string, object>();
        var fireCommand = Ioc.Resolve<ICommand>("Commands.Fire", ship);
        
        Assert.NotNull(fireCommand);
        Assert.IsType<FireCommand>(fireCommand);
    }
}

public class TorpedoMovementTest
{
    [Fact]
    public void MoveCommandMovesTorpedo()
    {
        var startPos = new Vector(0, 0);
        var velocity = new Vector(1, 2);
        var movingTorpedo = new MockMovingObject(startPos, velocity);
        var moveCommand = new MoveCommand(movingTorpedo);
        moveCommand.Execute();
        Assert.Equal(new Vector(1, 2), movingTorpedo.Position);
    }
}

internal class MockMovingObject : IMovingObject
{
    public Vector Position { get; set; }
    public Vector Velocity { get; }
    public MockMovingObject(Vector pos, Vector? vel = null)
    {
        Position = pos;
        Velocity = vel ?? new Vector(0, 0);
    }
}


internal class MockRotatingObject : IRotatingObject
{
    public Angle? Angle { get; set; }
    public Angle? AngularVelocity { get; }
    public MockRotatingObject(Angle? angle) => Angle = angle;
}
