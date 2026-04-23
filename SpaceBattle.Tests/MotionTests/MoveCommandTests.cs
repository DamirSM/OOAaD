using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class MoveCommandTests
{
    [Fact]
    public void Execute_MovesObjectCorrectly()
    {
        // Arrange
        var mock = new Mock<IMovingObject>();
        var initialPosition = new Vector(12, 5);
        var velocity = new Vector(-4, 1);
        mock.Setup(m => m.Position).Returns(initialPosition);
        mock.Setup(m => m.Velocity).Returns(velocity);
        // Настройка для проверки присвоения новой позиции
        Vector? newPosition = null;
        mock.SetupSet(m => m.Position = It.IsAny<Vector>())
            .Callback<Vector>(v => newPosition = v);

        var command = new MoveCommand(mock.Object);

        // Act
        command.Execute();

        // Assert
        Assert.Equal(new Vector(8, 6), newPosition);
        mock.VerifySet(m => m.Position = It.IsAny<Vector>(), Times.Once);
    }

    [Fact]
    public void Execute_WhenPositionIsNull_ThrowsException()
    {
        var mock = new Mock<IMovingObject>();
        mock.Setup(m => m.Position).Returns((Vector)null!);
        mock.Setup(m => m.Velocity).Returns(new Vector(1, 1));

        var command = new MoveCommand(mock.Object);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenVelocityIsNull_ThrowsException()
    {
        var mock = new Mock<IMovingObject>();
        mock.Setup(m => m.Position).Returns(new Vector(1, 1));
        mock.Setup(m => m.Velocity).Returns((Vector)null!);

        var command = new MoveCommand(mock.Object);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }

    [Fact]
    public void Execute_WhenCannotSetPosition_ThrowsException()
    {
        var mock = new Mock<IMovingObject>();
        mock.Setup(m => m.Position).Returns(new Vector(12, 5));
        mock.Setup(m => m.Velocity).Returns(new Vector(-4, 1));
        mock.SetupSet(m => m.Position = It.IsAny<Vector>())
            .Throws(new InvalidOperationException("Cannot set position"));

        var command = new MoveCommand(mock.Object);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }
}
