using System.Text.Json;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;

namespace SpaceBattle.Tests;

public class LoadCollisionDataCommandTests
{
    private const string TestJsonPath = "collision_data.json";

    [Fact]
    public void Execute_LoadsValidJson_RegistersShapeResolver()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var testData = new[]
        {
            new
            {
                Type = "Ship",
                LocalCircles = new[]
                {
                    new { Center = new[] { 0, 0 }, Radius = 1.5 },
                    new { Center = new[] { 1, 0 }, Radius = 0.8 }
                }
            }
        };
        string json = JsonSerializer.Serialize(testData);
        File.WriteAllText(TestJsonPath, json);

        var command = new LoadCollisionDataCommand();

        command.Execute();

        var circles = Ioc.Resolve<IReadOnlyList<Circle>>("Collision.Shape", "Ship");
        Assert.NotNull(circles);
        Assert.Equal(2, circles.Count);
        Assert.Equal(1.5, circles[0].Radius);
        Assert.Equal(0.8, circles[1].Radius);

        File.Delete(TestJsonPath);
    }

    [Fact]
    public void Execute_FileNotFound_ThrowsException()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        if (File.Exists(TestJsonPath))
        {
            File.Delete(TestJsonPath);
        }

        var command = new LoadCollisionDataCommand();

        Assert.Throws<FileNotFoundException>(() => command.Execute());
    }

    [Fact]
    public void Execute_InvalidJson_ThrowsException()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        File.WriteAllText(TestJsonPath, "invalid json {");

        var command = new LoadCollisionDataCommand();

        Assert.Throws<JsonException>(() => command.Execute());

        File.Delete(TestJsonPath);
    }

    [Fact]
    public void Execute_DeserializedDataIsNull_ThrowsException()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        File.WriteAllText(TestJsonPath, "null");

        var command = new LoadCollisionDataCommand();

        Assert.Throws<InvalidOperationException>(() => command.Execute());

        File.Delete(TestJsonPath);
    }
}
