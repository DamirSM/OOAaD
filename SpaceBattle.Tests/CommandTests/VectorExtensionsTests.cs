using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class VectorExtensionsTests
{
    [Fact]
    public void Dot_SameDimension_ReturnsCorrectValue()
    {
        var a = new Vector(1, 2, 3);
        var b = new Vector(4, 5, 6);

        double result = a.Dot(b);

        Assert.Equal(1 * 4 + 2 * 5 + 3 * 6, result);
    }

    [Fact]
    public void Dot_DifferentDimensions_ThrowsArgumentException()
    {
        var a = new Vector(1, 2);
        var b = new Vector(1, 2, 3);

        Assert.Throws<ArgumentException>(() => a.Dot(b));
    }
}
