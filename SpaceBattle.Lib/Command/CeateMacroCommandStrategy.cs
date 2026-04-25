using SpaceBattle.Lib;
using System.Linq;

public class CreateMacroCommandStrategy
{
    private readonly string _commandSpec;

    public CreateMacroCommandStrategy(string commandSpec)
    {
        _commandSpec = commandSpec;
    }

    public ICommand Resolve(object[] args)
    {
        var commandNames = Ioc.Resolve<string[]>(_commandSpec);

        if (commandNames == null || commandNames.Length == 0)
            throw new Exception("Spec not found or empty");

        var commands = commandNames
            .Select(name => Ioc.Resolve<ICommand>(name))
            .ToArray();
        return new MacroCommand(commands);
    }
}
