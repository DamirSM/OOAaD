using Xunit;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;
using System.IO;

namespace SpaceBattle.Tests;

public class InitCollisionSystemCommandTests
{
    [Fact]
    public void Execute_RegistersAllDependencies()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var initCmd = new InitCollisionSystemCommand();
        
        initCmd.Execute();
        
        // Проверяем, что ключи зарегистрированы
        var shape = Ioc.Resolve<IReadOnlyList<Circle>>("Collision.Shape", "Ship");
        Assert.NotNull(shape);
        
        var grid = Ioc.Resolve<SpatialGrid>("Collision.Grid");
        Assert.NotNull(grid);
        
        var snapshotCmd = Ioc.Resolve<ICommand>("Collision.SnapshotPositions");
        Assert.NotNull(snapshotCmd);
        
        var checkCmd = Ioc.Resolve<ICommand>("Collision.CheckAll");
        Assert.NotNull(checkCmd);
    }
}
