using System.Collections.Generic;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using Xunit;

public class RegisterIoCDependencyActionsStopTests
{
    [Fact]
    public void RegisterIoCDependencyActionsStop_Should_Resolve()
    {
        new RegisterIoCDependencyActionsStop().Execute();
        var gameObject = new Dictionary<string, object>();
        string cmdType = "Move";

        var command = Ioc.Resolve<ICommand>("Actions.Stop", gameObject, cmdType);

        Assert.NotNull(command);
        Assert.IsType<StopCommand>(command);
    }
}
