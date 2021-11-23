using System;
using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class RuntimeVariableTests
    {
        [Fact]
        public void RuntimeVariable_CanBeCreatedForDeclaredTypeInstance()
        {
            var declaration = VariableDeclaration.Create("aaa", SimpleTestPrototype.Lorem);
            var instance = SimpleTestPrototype.Lorem.CreateInstance();
            var variable = new RuntimeVariable(declaration, instance);

            variable.Declaration.Should().BeSameAs(declaration);
            variable.Instance.Should().BeSameAs(instance);
        }
        
        [Fact]
        public void RuntimeVariable_CannotBeCreatedForDifferentTypeInstance()
        {
            var declaration = VariableDeclaration.Create("aaa", SimpleTestPrototype.Lorem);
            var instance = SimpleTestPrototype.Ipsum.CreateInstance();
            Action variableCreationAttempt = () =>
            {
                var variable = new RuntimeVariable(declaration, instance);
            };

            variableCreationAttempt.Should().Throw<ArgumentException>()
                .Which.ParamName.Should().Be("instance");
        }
        
        [Fact]
        public void RuntimeVariable_CanBeCreatedForSubtypeOfDeclaredTypeInstance()
        {
            var declaration = VariableDeclaration.Create("aaa", SimpleTestPrototype.Ipsum);
            var instance = SimpleTestPrototype.SubIpsum.CreateInstance();
            var variable = new RuntimeVariable(declaration, instance);

            variable.Declaration.Should().BeSameAs(declaration);
            variable.Instance.Should().BeSameAs(instance);
        }
    }
}