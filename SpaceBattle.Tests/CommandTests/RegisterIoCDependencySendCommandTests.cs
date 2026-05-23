using Moq;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;

public class RegisterIoCDependencySendCommandTests
{
    [Fact]
    public void RegisterIoCDependencySendCommand_Should_Resolve_SendCommand()
    {
        var registerCommand = new RegisterIoCDependencySendCommand();
        registerCommand.Execute();

        var mockCommand = new Mock<ICommand>();
        var mockReceiver = new Mock<ICommandReceiver>();

        var resolvedCommand = Ioc.Resolve<ICommand>("Commands.Send", mockCommand.Object, mockReceiver.Object);

        Assert.NotNull(resolvedCommand);
        Assert.IsType<SendCommand>(resolvedCommand);
    }
}
