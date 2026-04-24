using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class RegisterIoCDependencyRotateCommandTests
{
    public RegisterIoCDependencyRotateCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        Angle.Denominator = 8;
    }

    [Fact]
    public void RegisterRotateCommandDependency_ShouldResolveRotateCommand()
    {
        var mockRotatingObject = new Mock<IRotatingObject>();
        mockRotatingObject.Setup(x => x.Angle).Returns(new Angle(1, 8));
        mockRotatingObject.Setup(x => x.AngularVelocity).Returns(new Angle(1, 8));
        mockRotatingObject.SetupSet(x => x.Angle = It.IsAny<Angle>());

        var rawGameObject = new Dictionary<string, object>();

        Ioc.Resolve<ICommand>(
            "IoC.Register",
            "Adapters.IRotatingObject",
            (object[] args) => mockRotatingObject.Object
        ).Execute();

        var registerCommand = new RegisterIoCDependencyRotateCommand();
        registerCommand.Execute();

        var resolvedCommand = Ioc.Resolve<ICommand>(
            "Commands.Rotate",
            rawGameObject
        );

        Assert.IsType<RotateCommand>(resolvedCommand);

        resolvedCommand.Execute();

        mockRotatingObject.VerifySet(
            x => x.Angle = It.Is<Angle>(a => a == new Angle(2, 8)),
            Times.Once
        );
    }
}
