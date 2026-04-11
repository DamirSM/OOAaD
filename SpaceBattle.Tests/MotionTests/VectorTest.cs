using Vector = SpaceBattle.Lib.Vector;

namespace SpaceBattle.Tests;

public class VectorTest
{
    [Fact]
    public void Constructor_WithEmptyArray_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Vector());
    }

    [Fact]
    public void Add_TwoVectorsWithSameDimensions_ReturnsCorrectSum()
    {
        Vector a = new Vector([1, -1, 2]);
        Vector b = new Vector([-1, 1, -2]);
        int[] expected = [0, 0, 0];

        var result = a + b;

        Assert.Equal(expected, result.Coordinates);
    }

    [Fact]
    public void Add_VectorsWithDifferentDimensions_FirstLonger_ThrowsArgumentException()
    {
        Vector a = new Vector([1, -1, 2]);
        Vector b = new Vector([-1, 1]);

        Assert.Throws<ArgumentException>(() => a + b);
    }

    [Fact]
    public void Add_VectorsWithDifferentDimensions_SecondLonger_ThrowsArgumentException()
    {
        Vector a = new Vector([1, -1]);
        Vector b = new Vector([-1, 1, -2]);

        Assert.Throws<ArgumentException>(() => a + b);
    }

    [Fact]
    public void Equals_TwoIdenticalVectors_ReturnsTrue()
    {
        Assert.True(new Vector([0, 0]).Equals(new Vector([0, 0])));
    }

    [Fact]
    public void Equals_TwoDifferentVectors_ReturnsFalse()
    {
        Assert.False(new Vector([0, 0]).Equals(new Vector([0, 1])));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var v = new Vector([1, 2, 3]);
        Assert.False(v.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        var v = new Vector([1, 2, 3]);
        Assert.False(v.Equals("not a vector"));
    }

    [Fact]
    public void Equals_WithSameObject_ReturnsTrue()
    {
        var v = new Vector([1, 2, 3]);
        Assert.True(v.Equals(v));
    }

    [Fact]
    public void EqualityOperator_TwoIdenticalVectors_ReturnsTrue()
    {
        var a = new Vector([0, 0, 1]);
        var b = new Vector([0, 0, 1]);

        bool result = a == b;

        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_OneNull_ReturnsFalse()
    {
        Vector? a = null;
        var b = new Vector([1, 2, 3]);

        Assert.False(a == b);
        Assert.False(b == a);
    }

    [Fact]
    public void InequalityOperator_TwoDifferentVectors_ReturnsTrue()
    {
        var a = new Vector([0, 0, 1]);
        var b = new Vector([0, 0, 2]);

        bool result = a != b;

        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_TwoNull_ReturnTrue()
    {
        Vector? a = null;
        Vector? b = null;

        Assert.True(a == b);
        Assert.True(b == a);
    }

    [Fact]
    public void InequalityOperator_OneNull_ReturnsTrue()
    {
        Vector? a = null;
        var b = new Vector([1, 2, 3]);

        Assert.True(a != b);
        Assert.True(b != a);
    }

    [Fact]
    public void GetHashCode_EqualVectors_ReturnsSameHashCode()
    {
        var v1 = new Vector([1, 2, 3]);
        var v2 = new Vector([1, 2, 3]);

        Assert.Equal(v1, v2);
        Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_EqualVectors_ReturnsNoSameHashCode()
    {
        var v1 = new Vector([1, 2, 3]);
        var v2 = new Vector([3, 2, 1]);

        Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void TestName()
    {
        var externalArr = new int[] { 0, 1 };
        var v = new Vector(externalArr);

        externalArr[0] = 999;

        Assert.Equal(0, v.Coordinates[0]);

    }
}
