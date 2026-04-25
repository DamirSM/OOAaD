using SpaceBattle.Lib;
using System.Linq;

public class RegisterIoCDependencyMacroMoveRotate : ICommand
{
    public void Execute()
    {
        // Macro.Move
        Ioc.Resolve<ICommand>(
            "IoC.Register",
            "Macro.Move",
            (Func<object[], object>)(_ =>
            {
                var commandNames = Ioc.Resolve<string[]>("Specs.Move");

                var commands = commandNames
                    .Select(name => Ioc.Resolve<ICommand>(name))
                    .ToArray();

                return new MacroCommand(commands);
            })
        ).Execute();

        // Macro.Rotate
        Ioc.Resolve<ICommand>(
            "IoC.Register",
            "Macro.Rotate",
            (Func<object[], object>)(_ =>
            {
                var commandNames = Ioc.Resolve<string[]>("Specs.Rotate");

                var commands = commandNames
                    .Select(name => Ioc.Resolve<ICommand>(name))
                    .ToArray();

                return new MacroCommand(commands);
            })
        ).Execute();
    }
}
