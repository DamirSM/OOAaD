using System.Text.Json;

namespace SpaceBattle.Lib.Command;

public class PrepareCollisionDataCommand : ICommand
{
    private class CircleDto
    {
        public int[] Center { get; set; } = Array.Empty<int>();  
        public double Radius { get; set; }
    }

    private class CollisionDataDto
    {
        public string Type { get; set; } = string.Empty;  
        public List<CircleDto> LocalCircles { get; set; } = new();  
    }

    public void Execute()
    {
        var data = new List<CollisionDataDto>
        {
            new CollisionDataDto
            {
                Type = "Ship",
                LocalCircles = new List<CircleDto>
                {
                    new CircleDto { Center = new int[] { 0, 0 }, Radius = 1.5 },
                    new CircleDto { Center = new int[] { 1, 0 }, Radius = 0.8 },
                    new CircleDto { Center = new int[] { -1, 0 }, Radius = 0.8 }
                }
            },
            new CollisionDataDto
            {
                Type = "Torpedo",
                LocalCircles = new List<CircleDto>
                {
                    new CircleDto { Center = new int[] { 0, 0 }, Radius = 0.5 }
                }
            }
        };
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("collision_data.json", json);
    }
}
