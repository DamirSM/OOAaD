using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;

public class RegisterDependencyCommandInjectableCommandTests
{
    [Fact]
    public void Register_And_Resolve_Should_Work_Without_Exceptions()
    {
        var registerCmd = new RegisterDependencyCommandInjectableCommand();

        registerCmd.Execute();

        var asCommand = Ioc.Resolve<ICommand>("Commands.CommandInjectable");
        var asInjectable = Ioc.Resolve<ICommandInjectable>("Commands.CommandInjectable");
        var asConcrete = Ioc.Resolve<CommandInjectableCommand>("Commands.CommandInjectable");

        Assert.NotNull(asCommand);
        Assert.NotNull(asInjectable);
        Assert.NotNull(asConcrete);
        Assert.IsType<CommandInjectableCommand>(asCommand);
        Assert.IsType<CommandInjectableCommand>(asInjectable);
        Assert.IsType<CommandInjectableCommand>(asConcrete);
    }
}
