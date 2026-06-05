using SpaceBattle.Lib;

namespace SpaceBattle.Lib;

public class Circle
{
    public Vector Center { get; }
    public double Radius { get; }
    public Circle(Vector center, double radius) => (Center, Radius) = (center, radius);
}

public static class VectorExtensions
{
    public static double Dot(this Vector a, Vector b)
    {
        if (a.Dimension != b.Dimension) throw new ArgumentException();
        return a.Coordinates.Zip(b.Coordinates, (x, y) => x * y).Sum();
    }
}

public class CollisionData
{
    public string Type { get; set; } = "";
    public List<Circle> LocalCircles { get; set; } = new();
}