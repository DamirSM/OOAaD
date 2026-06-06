using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;

namespace SpaceBattle.Tests;

public class InitCollisionSystemCommandTests
{
    [Fact]
    public void Execute_CreatesCollisionDataFile()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        string jsonPath = "collision_data.json";
        if (File.Exists(jsonPath))
        {
            File.Delete(jsonPath);
        }

        var initCmd = new InitCollisionSystemCommand();

        initCmd.Execute();

        Assert.True(File.Exists(jsonPath));
        var json = File.ReadAllText(jsonPath);
        Assert.Contains("Ship", json);
        Assert.Contains("Torpedo", json);

        File.Delete(jsonPath);
    }
    [Fact]
    public void Execute_RegistersAllDependencies()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var initCmd = new InitCollisionSystemCommand();

        initCmd.Execute();

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
