namespace SpaceBattle.Lib.Command;

public class RegisterDependencyCommandInjectableCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<ICommand>("IoC.Register", "Commands.CommandInjectable",
            (Func<object[], object>)(_ => new CommandInjectableCommand()));
    }
}
