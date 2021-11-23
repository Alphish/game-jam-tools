using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class VariableScopeTests
    {
        // -----
        // Tests
        // -----

        [Fact]
        public void VariableDeclarationScope_ShouldNotReturnDeclarationsWhenEmpty()
        {
            GivenRootScope();
            WithNoVariables();
            
            WhenGettingRootVariable("test");
            ThenExpectNoValue();
        }

        [Fact]
        public void VariableDeclarationScope_ShouldReturnDefinedDeclaration()
        {
            var loremInstance = SimpleTestPrototype.Lorem.CreateInstance();
            
            GivenRootScope();
            WithVariable("test", SimpleTestPrototype.Lorem, loremInstance);
            
            WhenGettingRootVariable("test");
            ThenExpectValue(loremInstance);
        }
        
        [Fact]
        public void ChildDeclarationScope_ShouldReturnOwnDeclaration()
        {
            var ipsumInstance = SimpleTestPrototype.Ipsum.CreateInstance();

            GivenRootScope();
            WithNoVariables();
            GivenChildScope();
            WithVariable("childTest", SimpleTestPrototype.Ipsum, ipsumInstance);
            
            WhenGettingChildVariable("childTest");
            ThenExpectValue(ipsumInstance);
        }
        
        [Fact]
        public void ParentDeclarationScope_ShouldNotReturnChildDeclarations()
        {
            var ipsumInstance = SimpleTestPrototype.Ipsum.CreateInstance();

            GivenRootScope();
            WithNoVariables();
            GivenChildScope();
            WithVariable("childTest", SimpleTestPrototype.Ipsum, ipsumInstance);
            
            WhenGettingRootVariable("childTest");
            ThenExpectNoValue();
        }
        
        [Fact]
        public void ChildDeclarationScope_ShouldFallbackToParentDeclaration()
        {
            var loremInstance = SimpleTestPrototype.Lorem.CreateInstance();
            
            GivenRootScope();
            WithVariable("test", SimpleTestPrototype.Lorem, loremInstance);
            GivenChildScope();
            WithNoVariables();
            
            WhenGettingChildVariable("test");
            ThenExpectValue(loremInstance);
        }
        
        [Fact]
        public void ChildDeclarationScope_ShouldOverrideParentDeclaration()
        {
            var ipsumInstance = SimpleTestPrototype.Ipsum.CreateInstance();
            var subipsumInstance = SimpleTestPrototype.SubIpsum.CreateInstance();
            
            GivenRootScope();
            WithVariable("test", SimpleTestPrototype.Ipsum, ipsumInstance);
            GivenChildScope();
            WithVariable("test", SimpleTestPrototype.SubIpsum, subipsumInstance);
            
            WhenGettingRootVariable("test");
            ThenExpectValue(ipsumInstance);
            
            WhenGettingChildVariable("test");
            ThenExpectValue(subipsumInstance);
        }
        
        // -----
        // Setup
        // -----

        private IVariableScope RootScope { get; set; } = default!;
        private IVariableScope? ChildScope { get; set; }
        
        private IInstance? ResultValue { get; set; }
        
        // Givens

        private void GivenRootScope()
            => RootScope = new VariableScope();

        private void GivenChildScope()
            => ChildScope = RootScope.CreateChildScope();

        private void WithNoVariables() { }
        
        private void WithVariable(string name, IPrototype prototype, IInstance instance)
        {
            var declaration = VariableDeclaration.Create(name, prototype);
            var scope = ChildScope ?? RootScope;
            scope.DefineVariable(declaration, instance);
        }
        
        // Whens

        private void WhenGettingRootVariable(string name)
            => ResultValue = RootScope.GetVariableValue(CodeName.From(name));

        private void WhenGettingChildVariable(string name)
            => ResultValue = ChildScope!.GetVariableValue(CodeName.From(name));
        
        // Thens

        private void ThenExpectValue(IInstance instance)
            => ResultValue.Should().Be(instance);

        private void ThenExpectNoValue()
            => ResultValue.Should().BeNull();
    }
}