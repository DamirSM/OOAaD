namespace SpaceBattle.Lib;

public class Angle
{
    public static int Denominator { get; set; }

    public int Numerator { get; set; }

    public Angle(int numerator, int denominator)
    {
        Denominator = denominator;

        Numerator = ((numerator % Denominator) + Denominator) % Denominator;
    }

    public static Angle operator +(Angle a, Angle b)
    {
        return new Angle(a.Numerator + b.Numerator, Denominator);
    }

    public override bool Equals(object? obj)
    {
        return obj != null
                && obj is Angle otherAngle
                && Numerator == otherAngle.Numerator;
    }

    public static bool operator ==(Angle? a, Angle? b)
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

    public static bool operator !=(Angle? a, Angle? b)
    {
        return !(a == b);
    }

    public override int GetHashCode() => Numerator.GetHashCode();

    public static implicit operator double(Angle angle)
    {
        return (double)angle.Numerator / Denominator * 2 * Math.PI;
    }
}
