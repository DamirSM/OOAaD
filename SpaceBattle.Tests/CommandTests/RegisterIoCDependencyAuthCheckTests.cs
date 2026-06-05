using Xunit;
using System;
using System.Collections.Generic;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;

namespace SpaceBattle.Tests.CommandTests
{
    public class RegisterIoCDependencyAuthCheckTests
    {
        [Fact]
        public void RegisterIoCDependencyAuthCheck_Successfully_Registers_Dependency()
        {
            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship1", new List<string> { "Action" } }
            };
            Ioc.Resolve<ICommand>("IoC.Register", "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();

            Assert.NotNull(Ioc.Resolve<object>("Authorization.Check", "player1", "Action", "object1"));
        }

        [Fact]
        public void AuthCheck_Returns_True_When_Player_Has_Global_Permissions()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";
            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "*", new List<string> { "Move", "Fire" } }
            };
            Ioc.Resolve<ICommand>("IoC.Register", "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();

            var result = Ioc.Resolve<bool>("Authorization.Check", subjectId, action, objectId);
            Assert.True(result);
        }

        [Fact]
        public void AuthCheck_Returns_False_When_Object_Not_In_Permissions()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";
            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship2", new List<string> { "Move", "Fire" } }
            };
            Ioc.Resolve<ICommand>("IoC.Register", "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();

            var result = Ioc.Resolve<bool>("Authorization.Check", subjectId, action, objectId);
            Assert.False(result);
        }

        [Fact]
        public void AuthCheck_Returns_True_When_Object_Has_Wildcard_Permission()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";
            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship1", new List<string> { "*" } }
            };
            Ioc.Resolve<ICommand>("IoC.Register", "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();

            var result = Ioc.Resolve<bool>("Authorization.Check", subjectId, action, objectId);
            Assert.True(result);
        }

        [Fact]
        public void AuthCheck_Returns_True_When_Object_Has_Specific_Permission()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";
            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship1", new List<string> { "Move", "Fire" } }
            };
            Ioc.Resolve<ICommand>("IoC.Register", "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();

            var result = Ioc.Resolve<bool>("Authorization.Check", subjectId, action, objectId);
            Assert.True(result);
        }

        [Fact]
        public void AuthCheck_Returns_False_When_Object_Does_Not_Have_Specific_Permission()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";
            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship1", new List<string> { "Fire", "Repair" } }
            };
            Ioc.Resolve<ICommand>("IoC.Register", "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();

            var result = Ioc.Resolve<bool>("Authorization.Check", subjectId, action, objectId);
            Assert.False(result);
        }
    }
}
