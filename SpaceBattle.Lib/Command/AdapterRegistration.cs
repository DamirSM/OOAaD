using System;
using System.Collections.Generic;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Interfaces;

namespace SpaceBattle.Lib.Command
{
    public static class AdapterRegistration
    {
        public static void Register()
        {
            Ioc.Resolve<ICommand>("IoC.Register", "Adapters.IMovingObject", (object[] args) =>
            {
                if (args[0] is not IDictionary<string, object> dict)
                    throw new ArgumentException("Expected IDictionary<string, object>");
                return new MovingObjectAdapter(dict);
            }).Execute();
        }
    }
}
