using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class RegisterIoCDependencyMoveCommandTests
    {
        public RegisterIoCDependencyMoveCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
        }

        [Fact]
        public void RegisterMoveCommandDependency_ShouldResolveMoveCommand()
        {
            var mockMovable = new Mock<IMovingObject>();
            mockMovable.Setup(m => m.Position).Returns(new Vector(0, 0));
            mockMovable.Setup(m => m.Velocity).Returns(new Vector(1, 1));
            mockMovable.SetupSet(m => m.Position = It.IsAny<Vector>());

            var rawGameObject = new Dictionary<string, object>();

            SpaceBattle.Lib.Ioc.Resolve<SpaceBattle.Lib.ICommand>("IoC.Register", "Adapters.IMovingObject",
                (object[] args) => mockMovable.Object
            ).Execute();

            var registerCommand = new RegisterIoCDependencyMoveCommand();
            registerCommand.Execute();

            var resolvedCommand = SpaceBattle.Lib.Ioc.Resolve<SpaceBattle.Lib.ICommand>("Commands.Move", rawGameObject);

            Assert.IsType<MoveCommand>(resolvedCommand);

            resolvedCommand.Execute();
        }
    }
}
