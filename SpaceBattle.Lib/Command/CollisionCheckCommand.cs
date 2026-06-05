namespace SpaceBattle.Lib.Command;

public class CollisionCheckCommand : ICommand
{
    private readonly Guid _objectId;
    private readonly Vector _oldPosition;
    private readonly Vector _newPosition;
    private readonly string _objectType;

    public CollisionCheckCommand(Guid id, Vector oldPos, Vector newPos, string type)
    {
        _objectId = id;
        _oldPosition = oldPos;
        _newPosition = newPos;
        _objectType = type;
    }

    public void Execute()
    {
        var registry = Ioc.Resolve<Dictionary<Guid, IDictionary<string, object>>>("Game.Registry");
        var myCircles = Ioc.Resolve<IReadOnlyList<Circle>>("Collision.Shape", _objectType);

        foreach (var kvp in registry)
        {
            if (kvp.Key == _objectId)
            {
                continue;
            }

            var other = kvp.Value;
            string otherType = (string)other["Type"];
            var otherCircles = Ioc.Resolve<IReadOnlyList<Circle>>("Collision.Shape", otherType);

            Vector otherPos = (Vector)other["Position"];
            bool collision = CollisionDetector.CheckCollision(
                _oldPosition, _newPosition, myCircles,
                otherPos, otherPos, otherCircles);

            if (collision)
            {
                Ioc.Resolve<ICommand>("Collision.Handle", _objectId, kvp.Key).Execute();
                break;
            }
        }
    }
}
