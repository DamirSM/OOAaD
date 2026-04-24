namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyMoveCommand : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<ICommand>("IoC.Register", "Commands.Move", (object[] args) =>
                {
                    var movingObject = args[0];
                    var movable = Ioc.Resolve<IMovingObject>("Adapters.IMovingObject", movingObject);
                    return new MoveCommand(movable);
                }
            ).Execute();
        }
    }
}
