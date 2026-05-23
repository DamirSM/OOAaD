namespace SpaceBattle.Lib.Interfaces;

public interface ICommandInjectable
{
    void Inject(ICommand command);
}
