using Xunit;
using Moq;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;

namespace SpaceBattle.Tests;

public class DestroyObjectsCommandTests
{
    [Fact]
    public void Execute_CallsDeleteForBothIds()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var deleteMock = new Mock<ICommand>();
        
        Ioc.Resolve<ICommand>("IoC.Register", "Game.Registry.Delete", 
            (Func<object[], object>)(args =>
            {
                var id = (Guid)args[0];
                return deleteMock.Object;
            })).Execute();

        var command = new DestroyObjectsCommand(id1, id2);
        
        command.Execute();
        
        deleteMock.Verify(c => c.Execute(), Times.Exactly(2));
    }
}