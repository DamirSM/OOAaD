using Xunit;
using Moq;
using SpaceBattle.Lib;

public class RegisterIoCDependencyMacroMoveRotateTests
{
    [Fact]
    public void Macro_Move_Should_Be_Resolved_And_Executed()
    {
        // Arrange
        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();

        Ioc.Resolve<ICommand>("IoC.Register", "MoveCmd1", (Func<object[], object>)(_ => cmd1.Object)).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "MoveCmd2", (Func<object[], object>)(_ => cmd2.Object)).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Specs.Move", (Func<object[], object>)(_ =>
            new[] { "MoveCmd1", "MoveCmd2" }
        )).Execute();

        new RegisterIoCDependencyMacroMoveRotate().Execute();

        // Act
        var macro = Ioc.Resolve<ICommand>("Macro.Move");
        macro.Execute();

        // Assert
        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Macro_Rotate_Should_Be_Resolved_And_Executed()
    {
        // Arrange
        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();

        Ioc.Resolve<ICommand>("IoC.Register", "RotateCmd1", (Func<object[], object>)(_ => cmd1.Object)).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "RotateCmd2", (Func<object[], object>)(_ => cmd2.Object)).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Specs.Rotate", (Func<object[], object>)(_ =>
            new[] { "RotateCmd1", "RotateCmd2" }
        )).Execute();

        new RegisterIoCDependencyMacroMoveRotate().Execute();

        // Act
        var macro = Ioc.Resolve<ICommand>("Macro.Rotate");
        macro.Execute();

        // Assert
        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
    }
}
