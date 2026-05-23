using System.Collections.Concurrent;

namespace SpaceBattle.Lib
{
    public static class Ioc
    {
        private static readonly ConcurrentDictionary<string, Func<object[], object>> _strategies = new();

        static Ioc()
        {
            _strategies["IoC.Register"] = args =>
            {
                var key = (string)args[0];
                var strategy = (Func<object[], object>)args[1];
                _strategies[key] = strategy;
                return new RegisterCommand(key, strategy);
            };
        }

        public static void Unregister(string key)
        {
            _strategies.TryRemove(key, out _);
        }

        public static T Resolve<T>(string key, params object[] args)
        {
            if (_strategies.TryGetValue(key, out var strategy))
            {
                return (T)strategy(args);
            }
            throw new ArgumentException($"Unknown IoC dependency key {key}");
        }

        private class RegisterCommand : ICommand
        {
            private readonly string _key;
            private readonly Func<object[], object> _strategy;
            public RegisterCommand(string key, Func<object[], object> strategy)
            {
                _key = key;
                _strategy = strategy;
            }
            public void Execute()
            {
                _strategies[_key] = _strategy;
            }
        }
    }
}
