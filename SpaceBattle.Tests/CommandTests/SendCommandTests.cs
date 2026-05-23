using Moq;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;

public class SendCommandTests
{
    [Fact]
    public void SendCommand_Should_Call_Receiver_With_Command()
    {
        var mockCommand = new Mock<ICommand>();
        var mockReceiver = new Mock<ICommandReceiver>();
        var sendCommand = new SendCommand(mockCommand.Object, mockReceiver.Object);

        sendCommand.Execute();

        mockReceiver.Verify(r => r.Receive(mockCommand.Object), Times.Once);
    }

    [Fact]
    public void SendCommand_Should_Throw_When_Receiver_Throws()
    {
        var mockCommand = new Mock<ICommand>();
        var mockReceiver = new Mock<ICommandReceiver>();
        mockReceiver.Setup(r => r.Receive(It.IsAny<ICommand>())).Throws(new Exception("Receiver error"));
        var sendCommand = new SendCommand(mockCommand.Object, mockReceiver.Object);

        Assert.Throws<Exception>(() => sendCommand.Execute());
    }
    [Fact]
    public void SendCommand_Constructor_Throws_WhenCommandIsNull()
    {
        var mockReceiver = new Mock<ICommandReceiver>();
        Assert.Throws<ArgumentNullException>(() => new SendCommand(null, mockReceiver.Object));
    }

    [Fact]
    public void SendCommand_Constructor_Throws_WhenReceiverIsNull()
    {
        var mockCommand = new Mock<ICommand>();
        Assert.Throws<ArgumentNullException>(() => new SendCommand(mockCommand.Object, null));
    }
}
