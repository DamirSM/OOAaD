using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;

namespace SpaceBattle.Tests;

public class CollisionCheckAllCommandTests
{
    private readonly SpatialGrid _grid;
    private readonly Dictionary<Guid, IDictionary<string, object>> _registry;

    public CollisionCheckAllCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        _registry = new Dictionary<Guid, IDictionary<string, object>>();
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry",
            (Func<object[], object>)(_ => _registry)).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Shape",
            (Func<object[], object>)(args => new List<Circle> { new Circle(new Vector(0, 0), 1.0) }.AsReadOnly())).Execute();

        // Регистрируем единственный экземпляр сетки
        _grid = new SpatialGrid(10.0);
        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Grid",
            (Func<object[], object>)(_ => _grid)).Execute();
    }

    [Fact]
    public void Execute_DetectsCollisionAndCallsHandler()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var obj1 = new Dictionary<string, object>
        {
            ["Id"] = id1,
            ["Type"] = "Ship",
            ["Position"] = new Vector(0, 0)
        };
        var obj2 = new Dictionary<string, object>
        {
            ["Id"] = id2,
            ["Type"] = "Ship",
            ["Position"] = new Vector(0, 0)
        };
        _registry[id1] = obj1;
        _registry[id2] = obj2;

        var snapshot = new Dictionary<Guid, Vector> { [id1] = new Vector(0, 0), [id2] = new Vector(0, 0) };
        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Snapshot", (Func<object[], object>)(_ => snapshot)).Execute();

        // Добавляем объекты в ту же сетку, которую будет использовать команда
        _grid.Add(id1, new Vector(0, 0));
        _grid.Add(id2, new Vector(0, 0));

        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry.Delete",
            (Func<object[], object>)(args => new DeleteObjectFromRegistryCommand((Guid)args[0]))).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Handle",
            (Func<object[], object>)(args => new DestroyObjectsCommand((Guid)args[0], (Guid)args[1]))).Execute();

        var command = new CollisionCheckAllCommand();
        command.Execute();

        Assert.False(_registry.ContainsKey(id1));
        Assert.False(_registry.ContainsKey(id2));
    }
}
