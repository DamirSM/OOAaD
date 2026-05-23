using System.Collections.Generic;
using Moq;                         
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;       
using Xunit;

public class RegisterIoCDependencyActionsStopTests
{
    [Fact]
    public void RegisterIoCDependencyActionsStop_Should_Resolve()
    {
        new RegisterIoCDependencyActionsStop().Execute();
        var gameObject = new Dictionary<string, object>();
        string cmdType = "Move";

        var command = Ioc.Resolve<ICommand>("Actions.Stop", gameObject, cmdType);

        Assert.NotNull(command);
        Assert.IsType<StopCommand>(command);
    }

    [Fact]
    public void StopCommand_Execute_RemovesCommandAndInjectsEmpty()
    {
        var gameObject = new Dictionary<string, object>();
        var cmdType = "Move";
        var injectableKey = $"repeatable{cmdType}";
        var mockInjectable = new Mock<ICommandInjectable>();
        gameObject[injectableKey] = mockInjectable.Object;

        var stopCommand = new StopCommand(gameObject, cmdType);

        stopCommand.Execute();

        mockInjectable.Verify(i => i.Inject(It.IsAny<EmptyCommand>()), Times.Once);
        Assert.False(gameObject.ContainsKey(injectableKey));
    }

    [Fact]
    public void StopCommand_Execute_Throws_WhenKeyNotFound()
    {
        var gameObject = new Dictionary<string, object>();
        var stopCommand = new StopCommand(gameObject, "Move");

        Assert.Throws<InvalidOperationException>(() => stopCommand.Execute());
    }
}
