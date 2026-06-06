using SpaceBattle.Lib.Interfaces;

namespace SpaceBattle.Lib;

public class CommandReceiver : ICommandReceiver
{
    public void Receive(ICommand command)
    {
        command.Execute();
    }
}
