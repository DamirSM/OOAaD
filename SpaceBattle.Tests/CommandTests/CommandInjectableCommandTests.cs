using Moq;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;

public class CommandInjectableCommandTests
{
    [Fact]
    public void Execute_Should_Run_Injected_Command()
    {
        var mockCommand = new Mock<ICommand>();
        var injectable = new CommandInjectableCommand();
        injectable.Inject(mockCommand.Object);

        injectable.Execute();

        mockCommand.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_Should_Throw_When_No_Command_Injected()
    {
        var injectable = new CommandInjectableCommand();

        Assert.Throws<InvalidOperationException>(() => injectable.Execute());
    }
}
