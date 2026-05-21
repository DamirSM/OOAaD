using SpaceBattle.Lib.Interfaces;

namespace SpaceBattle.Lib.Command;

public class RegisterIoCDependencySendCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<ICommand>("IoC.Register", "Commands.Send",
            (Func<object[], object>)(args =>
            {
                var command = (ICommand)args[0];
                var receiver = (ICommandReceiver)args[1];
                return new SendCommand(command, receiver);
            }));
    }
}
