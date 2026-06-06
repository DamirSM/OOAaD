namespace SpaceBattle.Tests
{
    public class InitScopeBasedIoCImplementationCommand : SpaceBattle.Lib.ICommand
    {
        public void Execute()
        {
            // Регистрируем корневой скоуп
            SpaceBattle.Lib.Ioc.Resolve<SpaceBattle.Lib.ICommand>("IoC.Register", "Scopes.Root",
                (object[] args) => new Dictionary<string, object>()
            ).Execute();

            // Регистрируем создание нового скоупа
            SpaceBattle.Lib.Ioc.Resolve<SpaceBattle.Lib.ICommand>("IoC.Register", "Scopes.New",
                (object[] args) => new Dictionary<string, object>()
            ).Execute();

            // Регистрируем команду установки текущего скоупа
            SpaceBattle.Lib.Ioc.Resolve<SpaceBattle.Lib.ICommand>("IoC.Register", "Scopes.Current.Set",
                (object[] args) =>
                {
                    var scope = args[0];
                    return new SetCurrentScopeCommand(scope);
                }
            ).Execute();

            // Регистрируем получение текущего скоупа (изначально – корневой)
            var rootScope = SpaceBattle.Lib.Ioc.Resolve<object>("Scopes.Root");
            SpaceBattle.Lib.Ioc.Resolve<SpaceBattle.Lib.ICommand>("IoC.Register", "Scopes.Current",
                (object[] args) => rootScope
            ).Execute();

            // Устанавливаем корневой скоуп как текущий
            var setCurrentCommand = SpaceBattle.Lib.Ioc.Resolve<SpaceBattle.Lib.ICommand>("Scopes.Current.Set", rootScope);
            setCurrentCommand.Execute();
        }

        private class SetCurrentScopeCommand : SpaceBattle.Lib.ICommand
        {
            private readonly object _scope;
            public SetCurrentScopeCommand(object scope) => _scope = scope;
            public void Execute()
            {
                // При выполнении команды меняем стратегию Scopes.Current
                SpaceBattle.Lib.Ioc.Resolve<SpaceBattle.Lib.ICommand>("IoC.Register", "Scopes.Current",
                    (object[] args) => _scope
                ).Execute();
            }
        }
    }
}
