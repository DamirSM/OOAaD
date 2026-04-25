namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyRotateCommand : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<ICommand>("IoC.Register", "Commands.Rotate", (object[] args) =>
                {
                    var rotatingObject = args[0];
                    var rotatable = Ioc.Resolve<IRotatingObject>("Adapters.IRotatingObject", rotatingObject);
                    return new RotateCommand(rotatable);
                }
            ).Execute();
        }
    }
}
