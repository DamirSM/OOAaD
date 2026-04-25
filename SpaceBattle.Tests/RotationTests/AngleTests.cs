using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class AngleTests
{
    [Fact]
    public void Add_TwoAngles_ReturnsNormalizedAngle()
    {
        var a = new Angle(5, 8);
        var b = new Angle(7, 8);
        var expected = new Angle(4, 8);

        var result = a + b;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equals_SameNormalizedAngles_ReturnsTrue()
    {
        var a = new Angle(15, 8);
        var b = new Angle(23, 8);

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void EqualityOperator_SameNormalizedAngles_ReturnsTrue()
    {
        var a = new Angle(15, 8);
        var b = new Angle(23, 8);

        Assert.True(a == b);
    }

    [Fact]
    public void Equals_SameNegativeNormalizedAngles_ReturnsTrue()
    {
        var a = new Angle(-9, 8);
        var b = new Angle(7, 8);

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void EqualityOperator_SameNegativeNormalizedAngles_ReturnsTrue()
    {
        var a = new Angle(-9, 8);
        var b = new Angle(7, 8);

        Assert.True(a == b);
    }

    [Fact]
    public void Equals_WithSameObject_ReturnsTrue()
    {
        var a = new Angle(1, 8);
        Assert.True(a.Equals(a));
    }

    [Fact]
    public void Equals_DifferentAngles_ReturnsFalse()
    {
        var a = new Angle(1, 8);
        var b = new Angle(2, 8);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var a = new Angle(1, 8);
        Assert.False(a.Equals(null));
    }

    [Fact]
    public void InequalityOperator_DifferentAngles_ReturnsTrue()
    {
        var a = new Angle(1, 8);
        var b = new Angle(2, 8);

        Assert.True(a != b);
    }

    [Fact]
    public void EqualityOperator_TwoNull_ReturnTrue()
    {
        Angle? a = null;
        Angle? b = null;

        Assert.True(a == b);
        Assert.True(b == a);
    }

    [Fact]
    public void InequalityOperator_OneNull_ReturnsTrue()
    {
        Angle? a = null;
        var b = new Angle(1, 8);

        Assert.True(a != b);
        Assert.True(b != a);
    }

    [Fact]
    public void GetHashCode_EqualAngles_ReturnsSameHashCode()
    {
        var a = new Angle(5, 8);
        var b = new Angle(5, 8);

        Assert.NotEqual(0, a.GetHashCode());
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void ImplicitConversionToDouble_AllowsMathCosAndSin()
    {
        var angle = new Angle(2, 8);

        double cos = Math.Cos(angle);
        double sin = Math.Sin(angle);

        Assert.Equal(0, cos, precision: 10);
        Assert.Equal(1, sin, precision: 10);
    }
}
