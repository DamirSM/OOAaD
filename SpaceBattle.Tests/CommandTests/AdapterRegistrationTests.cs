using Xunit;
using System;
using System.Collections.Generic;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;

namespace SpaceBattle.Tests.CommandTests
{
    public class AdapterRegistrationTests
    {
        [Fact]
        public void Register_AllowsResolvingIMovingObjectFromDictionary()
        {
            // Подготавливаем свежий Ioc (можно сбросить, но проще зарегистрировать временно)
            // Убедимся, что ключ "Adapters.IMovingObject" ещё не зарегистрирован
            // В вашем Ioc нет метода сброса, поэтому просто зарегистрируем и проверим.
            AdapterRegistration.Register();
            
            var torpedoDict = new Dictionary<string, object>
            {
                ["Position"] = new Vector(1, 2),
                ["Velocity"] = new Vector(10, 0)
            };
            
            // Act
            var movingObject = Ioc.Resolve<IMovingObject>("Adapters.IMovingObject", torpedoDict);
            
            // Assert
            Assert.NotNull(movingObject);
            Assert.Equal(new Vector(1, 2), movingObject.Position);
            Assert.Equal(new Vector(10, 0), movingObject.Velocity);
            
            // Проверяем, что изменение позиции через адаптер обновляет словарь
            movingObject.Position = new Vector(3, 4);
            Assert.Equal(new Vector(3, 4), torpedoDict["Position"]);
        }
        
        [Fact]
        public void Register_ThrowsWhenArgumentIsNotDictionary()
        {
            AdapterRegistration.Register();
            
            Assert.Throws<ArgumentException>(() =>
                Ioc.Resolve<IMovingObject>("Adapters.IMovingObject", "not a dictionary"));
        }
    }
}
