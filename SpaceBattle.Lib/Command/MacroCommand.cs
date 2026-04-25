namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private readonly ICommand[] _commands;

    public MacroCommand(ICommand[] commands)
    {
        if (commands == null || commands.Length == 0)
        {
            throw new ArgumentException("Пустой или некорректный массив команд");
        }
        _commands = commands.ToArray();
    }

    public void Execute()
    {
        ExecuteRecursive(0);
    }

    private void ExecuteRecursive(int index)
    {
        if (index >= _commands.Length)
            return;

        _commands[index].Execute();

        ExecuteRecursive(index + 1);
    }
}