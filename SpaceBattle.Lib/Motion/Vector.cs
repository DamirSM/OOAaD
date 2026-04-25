
namespace SpaceBattle.Lib;

public class Vector
{
    public int[] Coordinates { get; }
    public Vector(params int[]? coord)
    {
        if (coord == null || coord.Length == 0)
        {
            throw new ArgumentException("Некорректная размерноcть вектора");
        }

        Coordinates = new int[coord.Length];
        Array.Copy(coord, Coordinates, coord.Length);
    }

    public override bool Equals(object? obj)
    {
        return obj != null
                && obj is Vector otherVector
                && Coordinates.SequenceEqual(otherVector.Coordinates);
    }

    public static Vector operator +(Vector vector1, Vector vector2)
    {
        if (vector1.Dimension != vector2.Dimension)
        {
            throw new ArgumentException("Размерности векторов не совпадают.");
        }

        var resultCoordinates = vector1.Coordinates
            .Select((c, index) => c + vector2.Coordinates[index])
            .ToArray();

        return new Vector(resultCoordinates);
    }

    public static bool operator ==(Vector? a, Vector? b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Vector? a, Vector? b)
    {
        return !(a == b);
    }

    public int Dimension => Coordinates.Length;

    public override int GetHashCode()
    {
        int hash = 76;

        foreach (var c in Coordinates)
        {
            hash = hash * 89 + c;
        }

        return hash;
    }
}
