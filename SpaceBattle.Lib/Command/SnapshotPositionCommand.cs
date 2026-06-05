namespace SpaceBattle.Lib.Command;

public class SnapshotPositionsCommand : ICommand
{
    public void Execute()
    {
        var registry = Ioc.Resolve<Dictionary<Guid, IDictionary<string, object>>>("Game.Registry");
        var snapshot = new Dictionary<Guid, Vector>();
        foreach (var kv in registry)
        {
            if (kv.Value.ContainsKey("Position"))
                snapshot[kv.Key] = (Vector)kv.Value["Position"];
        }
        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Snapshot", (Func<object[], object>)(_ => snapshot)).Execute();
    }
}