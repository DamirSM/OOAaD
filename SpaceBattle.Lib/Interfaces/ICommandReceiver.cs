namespace SpaceBattle.Lib.Interfaces;

public interface ICommandReceiver
{
    void Receive(ICommand command);
}
