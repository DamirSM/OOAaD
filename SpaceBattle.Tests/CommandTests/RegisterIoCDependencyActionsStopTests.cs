using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;

public class RegisterIoCDependencyActionsStopTests
{
    [Fact]
    public void RegisterIoCDependencyActionsStop_Should_Resolve()
    {
        var registerCommand = new RegisterIoCDependencyActionsStop();
        registerCommand.Execute();
        var order = new Dictionary<string, object>();

        var command = Ioc.Resolve<ICommand>("Actions.Stop", order);

        Assert.NotNull(command);
    }
}
