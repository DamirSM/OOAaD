using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;

public class RegisterIoCDependencyActionsStartTests
{
    [Fact]
    public void RegisterIoCDependencyActionsStart_Should_Resolve()
    {
        var registerCommand = new RegisterIoCDependencyActionsStart();
        registerCommand.Execute();
        var order = new Dictionary<string, object>();

        var command = Ioc.Resolve<ICommand>("Actions.Start", order);

        Assert.NotNull(command);
    }
}
