namespace SpaceBattle.Lib.Command;

public class InitCollisionSystemCommand : ICommand
{
    public void Execute()
    {
        new PrepareCollisionDataCommand().Execute();
        new LoadCollisionDataCommand().Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Grid",
            (Func<object[], object>)(_ => new SpatialGrid(5.0))).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Handle",
            (Func<object[], object>)(args => new DestroyObjectsCommand((Guid)args[0], (Guid)args[1]))).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Collision.SnapshotPositions",
            (Func<object[], object>)(_ => new SnapshotPositionsCommand())).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Collision.CheckAll",
            (Func<object[], object>)(_ => new CollisionCheckAllCommand())).Execute();
    }
}
