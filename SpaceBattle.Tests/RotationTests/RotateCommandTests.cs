using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class RotateCommandTests
{
    private static void SetDefaultDenominator() => Angle.Denominator = 8;

    [Fact]
    public void Execute_ValidAngleAndVelocity_UpdatesAngleTo90Degrees()
    {
        SetDefaultDenominator();
        var initialAngle = new Angle(1, 8);
        var angularVelocity = new Angle(1, 8);
        var expectedAngle = new Angle(2, 8);

        var mock = new Mock<IRotatingObject>();
        mock.SetupProperty(x => x.Angle, initialAngle);
        mock.Setup(x => x.AngularVelocity).Returns(angularVelocity);

        var command = new RotateCommand(mock.Object);
        command.Execute();

        Assert.Equal(expectedAngle, mock.Object.Angle);
    }

    [Fact]
    public void Execute_CannotGetAngle_ThrowsInvalidOperationException()
    {
        var mock = new Mock<IRotatingObject>();
        mock.Setup(x => x.Angle).Returns((Angle?)null);

        var command = new RotateCommand(mock.Object);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }

    [Fact]
    public void Execute_CannotGetAngularVelocity_ThrowsInvalidOperationException()
    {
        SetDefaultDenominator();
        var mock = new Mock<IRotatingObject>();
        mock.Setup(x => x.Angle).Returns(new Angle(1, 8));
        mock.Setup(x => x.AngularVelocity).Returns((Angle?)null);

        var command = new RotateCommand(mock.Object);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }

    [Fact]
    public void Execute_CannotChangeAngle_ThrowsInvalidOperationException()
    {
        SetDefaultDenominator();
        var initialAngle = new Angle(1, 8);
        var angularVelocity = new Angle(1, 8);

        var mock = new Mock<IRotatingObject>();
        mock.Setup(x => x.Angle).Returns(initialAngle);
        mock.Setup(x => x.AngularVelocity).Returns(angularVelocity);

        mock.SetupSet(x => x.Angle = It.IsAny<Angle?>()).Throws<InvalidOperationException>();

        var command = new RotateCommand(mock.Object);

        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }
}
