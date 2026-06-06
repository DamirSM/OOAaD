namespace SpaceBattle.Lib.Command;

public class CollisionCheckAllCommand : ICommand
{
    public void Execute()
    {
        var registry = Ioc.Resolve<Dictionary<Guid, IDictionary<string, object>>>("Game.Registry");
        var snapshot = Ioc.Resolve<Dictionary<Guid, Vector>>("Collision.Snapshot");
        var grid = Ioc.Resolve<SpatialGrid>("Collision.Grid");

        foreach (var kv in registry)
        {
            var id = kv.Key;
            var obj = kv.Value;
            if (!obj.ContainsKey("Position"))
            {
                continue;
            }

            Vector oldPos = snapshot.GetValueOrDefault(id, (Vector)obj["Position"]);
            Vector newPos = (Vector)obj["Position"];
            string type = (string)obj["Type"];
            grid.Update(kv.Key, oldPos, newPos);
            var circles = Ioc.Resolve<IReadOnlyList<Circle>>("Collision.Shape", type);

            var neighbors = grid.GetNearby(id, newPos, 10.0);
            foreach (var otherId in neighbors)
            {
                if (otherId == id)
                {
                    continue;
                }

                var otherObj = registry[otherId];
                if (!otherObj.ContainsKey("Position"))
                {
                    continue;
                }

                Vector otherOldPos = snapshot.GetValueOrDefault(otherId, (Vector)otherObj["Position"]);
                Vector otherNewPos = (Vector)otherObj["Position"];
                string otherType = (string)otherObj["Type"];
                var otherCircles = Ioc.Resolve<IReadOnlyList<Circle>>("Collision.Shape", otherType);

                if (CollisionDetector.CheckCollision(oldPos, newPos, circles, otherOldPos, otherNewPos, otherCircles))
                {
                    Ioc.Resolve<ICommand>("Collision.Handle", id, otherId).Execute();
                    break; // объект уничтожен, дальше не проверяем
                }
            }
        }
    }
}
