using Moq;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;
using Xunit;

public class RegisterIoCDependencyActionsStartTests
{
    [Fact]
    public void RegisterIoCDependencyActionsStart_Should_Resolve_And_Execute_Without_Errors()
    {
        var mockReceiver = new Mock<ICommandReceiver>();

        Ioc.Resolve<ICommand>("IoC.Register", "Specs.Move",
            (Func<object[], object>)(_ => new string[] { "Commands.Move" })).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Commands.Move",
            (Func<object[], object>)(_ => new Mock<ICommand>().Object)).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Commands.CommandInjectable",
            (Func<object[], object>)(_ => new CommandInjectableCommand())).Execute();

        new RegisterIoCDependencyActionsStart().Execute();

        var gameObject = new Dictionary<string, object>
        {
            ["Receiver"] = mockReceiver.Object
        };
        string cmdType = "Move";

        var command = Ioc.Resolve<ICommand>("Actions.Start", gameObject, cmdType);

        Assert.NotNull(command);
        Assert.IsType<StartCommand>(command);

        command.Execute();
        Assert.True(true);
    }
}
