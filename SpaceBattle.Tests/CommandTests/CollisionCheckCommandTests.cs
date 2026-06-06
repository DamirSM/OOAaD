using Moq;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;

namespace SpaceBattle.Tests;

public class CollisionCheckCommandTests
{
    private readonly Dictionary<Guid, IDictionary<string, object>> _registry;

    public CollisionCheckCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        _registry = new Dictionary<Guid, IDictionary<string, object>>();
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry",
            (Func<object[], object>)(_ => _registry)).Execute();

        // Регистрируем обработчик коллизий
        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Handle",
            (Func<object[], object>)(args => new DestroyObjectsCommand((Guid)args[0], (Guid)args[1]))).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry.Delete",
            (Func<object[], object>)(args => new Mock<ICommand>().Object)).Execute();
    }

    [Fact]
    public void Execute_NoCollision_DoesNothing()
    {
        // Arrange
        var id = Guid.NewGuid();
        var otherId = Guid.NewGuid();

        _registry[id] = new Dictionary<string, object>
        {
            ["Type"] = "Ship",
            ["Position"] = new Vector(0, 0)
        };
        _registry[otherId] = new Dictionary<string, object>
        {
            ["Type"] = "Ship",
            ["Position"] = new Vector(10, 0)
        };

        var shapesRegistered = false;
        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Shape",
            (Func<object[], object>)(args =>
            {
                shapesRegistered = !shapesRegistered;
                return new List<Circle> { new Circle(new Vector(0, 0), 1.0) }.AsReadOnly();
            })).Execute();

        var command = new CollisionCheckCommand(id, new Vector(0, 0), new Vector(0, 0), "Ship");

        // Act
        command.Execute();

        // Assert
        Assert.True(_registry.ContainsKey(id));
        Assert.True(_registry.ContainsKey(otherId));
    }

    [Fact]
    public void Execute_CollisionDetected_CallsHandler()
    {
        // Arrange
        var id = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        var handlerCalled = false;

        _registry[id] = new Dictionary<string, object>
        {
            ["Type"] = "Ship",
            ["Position"] = new Vector(0, 0)
        };
        _registry[otherId] = new Dictionary<string, object>
        {
            ["Type"] = "Ship",
            ["Position"] = new Vector(0, 0)
        };

        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Shape",
            (Func<object[], object>)(args =>
            {
                return new List<Circle> { new Circle(new Vector(0, 0), 1.0) }.AsReadOnly();
            })).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Handle",
            (Func<object[], object>)(args =>
            {
                handlerCalled = true;
                return new DestroyObjectsCommand((Guid)args[0], (Guid)args[1]);
            })).Execute();

        var command = new CollisionCheckCommand(id, new Vector(0, 0), new Vector(0, 0), "Ship");

        // Act
        command.Execute();

        // Assert
        Assert.True(handlerCalled);
    }
}
