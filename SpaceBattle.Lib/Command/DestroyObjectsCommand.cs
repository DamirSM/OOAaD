namespace SpaceBattle.Lib.Command;

public class DestroyObjectsCommand : ICommand
{
    private readonly Guid _id1;
    private readonly Guid _id2;
    public DestroyObjectsCommand(Guid id1, Guid id2) => (_id1, _id2) = (id1, id2);
    public void Execute()
    {
        Ioc.Resolve<ICommand>("Game.Registry.Delete", _id1).Execute();
        Ioc.Resolve<ICommand>("Game.Registry.Delete", _id2).Execute();
    }
}
