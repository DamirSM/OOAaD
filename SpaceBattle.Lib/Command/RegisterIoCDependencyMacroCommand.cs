using SpaceBattle.Lib;

public class RegisterIoCDependencyMacroCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<ICommand>(
            "IoC.Register",
            "Commands.Macro",
            (Func<object[], object>)(args =>
            {
                var commands = args.Cast<ICommand>().ToArray();
                return new MacroCommand(commands);
            })
        ).Execute();
    }
}
