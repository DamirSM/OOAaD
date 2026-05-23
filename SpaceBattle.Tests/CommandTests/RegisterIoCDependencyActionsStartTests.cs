using System;
using System.Collections.Generic;
using Moq;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;
using Xunit;

public class RegisterIoCDependencyActionsStartTests : IDisposable
{
    private static readonly string[] KeysToClean =
    {
        "Specs.Move",
        "Commands.Move",
        "Commands.CommandInjectable",
        "Macro.Move",
        "Actions.Start"
    };

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

        Ioc.Resolve<ICommand>("IoC.Register", "Macro.Move",
            (Func<object[], object>)(args =>
            {
                var cmd = (ICommand)args[0];
                var send = (ICommand)args[1];
                return new MacroCommand(new ICommand[] { cmd, send });
            })).Execute();

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
    }

    public void Dispose()
    {
        foreach (var key in KeysToClean)
        {
            Ioc.Unregister(key); // чистка после теста
        }
    }
    
    [Fact]
    public void StartCommand_Constructor_Throws_WhenReceiverKeyMissing()
    {
        var gameObject = new Dictionary<string, object>(); 
        Assert.Throws<InvalidOperationException>(() => new StartCommand(gameObject, "Move"));
    }

    [Fact]
    public void StartCommand_Constructor_Throws_WhenReceiverValueIsWrongType()
    {
        var gameObject = new Dictionary<string, object>
        {
            ["Receiver"] = "not a receiver" 
        };
        Assert.Throws<InvalidOperationException>(() => new StartCommand(gameObject, "Move"));
    }
}
