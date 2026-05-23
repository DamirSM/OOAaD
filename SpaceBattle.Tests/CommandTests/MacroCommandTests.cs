using Xunit;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class MacroCommandTest
    {
        [Fact]
        public void Execute_ShouldExecuteAllSubcommands()
        {
            var command1 = new Mock<ICommand>();
            var command2 = new Mock<ICommand>();
            var command3 = new Mock<ICommand>();

            var macroCommand = new MacroCommand(new ICommand[]
            {
                command1.Object,
                command2.Object,
                command3.Object
            });

            macroCommand.Execute();

            command1.Verify(c => c.Execute(), Times.Once);
            command2.Verify(c => c.Execute(), Times.Once);
            command3.Verify(c => c.Execute(), Times.Once);
        }

        [Fact]
        public void Execute_ShouldThrow_WhenAnySubcommandThrows()
        {
            var command1 = new Mock<ICommand>();
            var command2 = new Mock<ICommand>();
            var command3 = new Mock<ICommand>();

            command2.Setup(c => c.Execute()).Throws(new Exception());

            var macroCommand = new MacroCommand(new ICommand[]
            {
                command1.Object,
                command2.Object,
                command3.Object
            });

            Assert.Throws<Exception>(() => macroCommand.Execute());

            command1.Verify(c => c.Execute(), Times.Once);
            command2.Verify(c => c.Execute(), Times.Once);
            command3.Verify(c => c.Execute(), Times.Never);
        }
        [Fact]
        public void Constructor_Throws_WhenCommandsIsNull()
        {
            Assert.Throws<ArgumentException>(() => new MacroCommand(null));
        }

        [Fact]
        public void Constructor_Throws_WhenCommandsArrayIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new MacroCommand(Array.Empty<ICommand>()));
        }
    }
}
