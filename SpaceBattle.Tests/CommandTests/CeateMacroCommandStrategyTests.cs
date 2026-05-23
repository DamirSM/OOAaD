using Xunit;
using Moq;
using SpaceBattle.Lib;

public class CeateMacroCommandStrategyTestsTests
{
    [Fact]
    public void Resolve_Should_Create_And_Execute_MacroCommand()
    {
        // Arrange
        var move = new Mock<ICommand>();
        var log = new Mock<ICommand>();

        Ioc.Resolve<ICommand>("IoC.Register", "Move", (Func<object[], object>)(_ => move.Object)).Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Log", (Func<object[], object>)(_ => log.Object)).Execute();

        Ioc.Resolve<ICommand>("IoC.Register", "Specs.Macro.Test", (Func<object[], object>)(_ =>
            new[] { "Move", "Log" }
        )).Execute();

        var strategy = new CreateMacroCommandStrategy("Specs.Macro.Test");

        // Act
        var macro = strategy.Resolve(Array.Empty<object>());

        macro.Execute();

        // Assert
        move.Verify(m => m.Execute(), Times.Once);
        log.Verify(m => m.Execute(), Times.Once);
    }
    [Fact]
    public void Resolve_Should_Throw_When_Spec_Not_Found()
    {
        // Arrange
        var strategy = new CreateMacroCommandStrategy("Specs.Macro.Missing");

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
        {
            strategy.Resolve(Array.Empty<object>());
        });
    }
    [Fact]
    public void Resolve_Throws_When_Spec_Returns_EmptyArray()
    {
        Ioc.Resolve<ICommand>("IoC.Register", "Specs.Macro.Empty", (Func<object[], object>)(_ => Array.Empty<string>())).Execute();
        var strategy = new CreateMacroCommandStrategy("Specs.Macro.Empty");
        Assert.Throws<Exception>(() => strategy.Resolve(Array.Empty<object>()));
    }
}
