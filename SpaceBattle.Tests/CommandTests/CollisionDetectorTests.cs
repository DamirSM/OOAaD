using Xunit;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;

namespace SpaceBattle.Tests;

public class CollisionDetectorTests
{
    [Fact]
    public void CheckCollision_StaticCircles_Intersecting_ReturnsTrue()
    {
        var circles1 = new[] { new Circle(new Vector(0, 0), 1.0) };
        var circles2 = new[] { new Circle(new Vector(1, 0), 1.0) };
        Vector start1 = new Vector(0, 0), end1 = new Vector(0, 0);
        Vector start2 = new Vector(1, 0), end2 = new Vector(1, 0);

        bool result = CollisionDetector.CheckCollision(start1, end1, circles1, start2, end2, circles2);

        Assert.True(result);
    }

    [Fact]
    public void CheckCollision_StaticCircles_NotIntersecting_ReturnsFalse()
    {
        var circles1 = new[] { new Circle(new Vector(0, 0), 1.0) };
        var circles2 = new[] { new Circle(new Vector(3, 0), 1.0) };
        Vector start1 = new Vector(0, 0), end1 = new Vector(0, 0);
        Vector start2 = new Vector(3, 0), end2 = new Vector(3, 0);

        bool result = CollisionDetector.CheckCollision(start1, end1, circles1, start2, end2, circles2);

        Assert.False(result);
    }

    [Fact]
    public void CheckCollision_MovingCircles_WillCollide_ReturnsTrue()
    {
        var circles1 = new[] { new Circle(new Vector(0, 0), 1.0) };
        var circles2 = new[] { new Circle(new Vector(5, 0), 1.0) };
        Vector start1 = new Vector(0, 0), end1 = new Vector(5, 0);
        Vector start2 = new Vector(5, 0), end2 = new Vector(0, 0);

        bool result = CollisionDetector.CheckCollision(start1, end1, circles1, start2, end2, circles2, dt: 1.0);

        Assert.True(result);
    }

    [Fact]
    public void CheckCollision_MovingCircles_WillNotCollide_ReturnsFalse()
    {
        var circles1 = new[] { new Circle(new Vector(0, 0), 1.0) };
        var circles2 = new[] { new Circle(new Vector(10, 0), 1.0) };
        Vector start1 = new Vector(0, 0), end1 = new Vector(5, 0);
        Vector start2 = new Vector(10, 0), end2 = new Vector(5, 0);

        bool result = CollisionDetector.CheckCollision(start1, end1, circles1, start2, end2, circles2, dt: 1.0);

        Assert.False(result);
    }

    [Fact]
    public void CheckCollision_MultipleCircles_AnyPairIntersects_ReturnsTrue()
    {
        var circles1 = new[]
        {
            new Circle(new Vector(0, 0), 0.5),
            new Circle(new Vector(2, 0), 0.5)
        };
        var circles2 = new[]
        {
            new Circle(new Vector(1, 0), 0.5),
            new Circle(new Vector(3, 0), 0.5)
        };
        Vector start1 = new Vector(0, 0), end1 = new Vector(0, 0);
        Vector start2 = new Vector(1, 0), end2 = new Vector(1, 0);

        bool result = CollisionDetector.CheckCollision(start1, end1, circles1, start2, end2, circles2);

        Assert.True(result);
    }
}