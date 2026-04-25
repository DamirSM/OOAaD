using Xunit;
using Moq;
using SpaceBattle.Lib;


namespace SpaceBattle.Tests;
public class RegisterIoCDependencyMacroCommandTests{
    [Fact]
    public void Macro_Dependency_Should_Be_Resolved()
    {
        new RegisterIoCDependencyMacroCommand().Execute();

        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();

        var macro = Ioc.Resolve<ICommand>(
            "Commands.Macro",
            new ICommand[] { cmd1.Object, cmd2.Object }
        );

        Assert.IsType<MacroCommand>(macro);
    }
}
