using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;

namespace SpaceBattle.Tests;

public class SnapshotPositionsCommandTests
{
    [Fact]
    public void Execute_SavesPositionsToIoC()
    {
        // Arrange
        var registry = new Dictionary<Guid, IDictionary<string, object>>();
        var objId = Guid.NewGuid();
        var obj = new Dictionary<string, object> { ["Position"] = new Vector(1, 2) };
        registry[objId] = obj;

        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry", (Func<object[], object>)(_ => registry)).Execute();

        Dictionary<Guid, Vector>? snapshot = null;
        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Snapshot", (Func<object[], object>)(args => snapshot = (Dictionary<Guid, Vector>)args[0])).Execute();

        var command = new SnapshotPositionsCommand();

        // Act
        command.Execute();

        // Assert
        snapshot = Ioc.Resolve<Dictionary<Guid, Vector>>("Collision.Snapshot");
        Assert.NotNull(snapshot);
        Assert.Equal(new Vector(1, 2), snapshot[objId]);
    }

    [Fact]
    public void Execute_ObjectsWithoutPosition_AreIgnored()
    {
        var registry = new Dictionary<Guid, IDictionary<string, object>>();
        var objId = Guid.NewGuid();
        var obj = new Dictionary<string, object> { ["Name"] = "NoPosition" };
        registry[objId] = obj;

        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry", (Func<object[], object>)(_ => registry)).Execute();

        var command = new SnapshotPositionsCommand();

        command.Execute();

        var snapshot = Ioc.Resolve<Dictionary<Guid, Vector>>("Collision.Snapshot");
        Assert.False(snapshot.ContainsKey(objId));
    }
}
