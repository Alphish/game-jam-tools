using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class VariableDeclarationScopeTests
    {
        // -----
        // Tests
        // -----

        [Fact]
        public void VariableDeclarationScope_ShouldNotReturnDeclarationsWhenEmpty()
        {
            GivenRootScope();
            WithNoVariableDeclarations();
            
            WhenGettingRootDeclaration("test");
            ThenExpectNoDeclaration();
        }

        [Fact]
        public void VariableDeclarationScope_ShouldReturnDefinedDeclaration()
        {
            GivenRootScope();
            WithVariableDeclaration("test", SimpleTestPrototype.Lorem);
            
            WhenGettingRootDeclaration("test");
            ThenExpectDeclaration("test", SimpleTestPrototype.Lorem);
        }
        
        [Fact]
        public void ChildDeclarationScope_ShouldReturnOwnDeclaration()
        {
            GivenRootScope();
            WithNoVariableDeclarations();
            GivenChildScope();
            WithVariableDeclaration("childTest", SimpleTestPrototype.Ipsum);
            
            WhenGettingChildDeclaration("childTest");
            ThenExpectDeclaration("childTest", SimpleTestPrototype.Ipsum);
        }
        
        [Fact]
        public void ParentDeclarationScope_ShouldNotReturnChildDeclarations()
        {
            GivenRootScope();
            WithNoVariableDeclarations();
            GivenChildScope();
            WithVariableDeclaration("childTest", SimpleTestPrototype.Ipsum);
            
            WhenGettingRootDeclaration("childTest");
            ThenExpectNoDeclaration();
        }
        
        [Fact]
        public void ChildDeclarationScope_ShouldFallbackToParentDeclaration()
        {
            GivenRootScope();
            WithVariableDeclaration("test", SimpleTestPrototype.Lorem);
            GivenChildScope();
            WithNoVariableDeclarations();
            
            WhenGettingChildDeclaration("test");
            ThenExpectDeclaration("test", SimpleTestPrototype.Lorem);
        }
        
        [Fact]
        public void ChildDeclarationScope_ShouldOverrideParentDeclaration()
        {
            GivenRootScope();
            WithVariableDeclaration("test", SimpleTestPrototype.Ipsum);
            GivenChildScope();
            WithVariableDeclaration("test", SimpleTestPrototype.SubIpsum);
            
            WhenGettingRootDeclaration("test");
            ThenExpectDeclaration("test", SimpleTestPrototype.Ipsum);
            
            WhenGettingChildDeclaration("test");
            ThenExpectDeclaration("test", SimpleTestPrototype.SubIpsum);
        }
        
        // -----
        // Setup
        // -----

        private IVariableDeclarationScope RootScope { get; set; } = default!;
        private IVariableDeclarationScope? ChildScope { get; set; }
        
        private VariableDeclaration? ResultDeclaration { get; set; }
        
        // Givens

        private void GivenRootScope()
            => RootScope = new VariableDeclarationScope();

        private void GivenChildScope()
            => ChildScope = RootScope.CreateChildScope();

        private void WithNoVariableDeclarations() { }
        
        private void WithVariableDeclaration(string name, IPrototype prototype)
        {
            var declaration = VariableDeclaration.Create(name, prototype);
            var scope = ChildScope ?? RootScope;
            scope.DeclareVariable(declaration);
        }
        
        // Whens

        private void WhenGettingRootDeclaration(string name)
            => ResultDeclaration = RootScope.GetDeclaration(CodeName.From(name));

        private void WhenGettingChildDeclaration(string name)
            => ResultDeclaration = ChildScope!.GetDeclaration(CodeName.From(name));
        
        // Thens

        private void ThenExpectDeclaration(string name, IPrototype prototype)
        {
            var variableName = CodeName.From(name);
            ResultDeclaration!.Name.Should().Be(variableName);
            ResultDeclaration!.Prototype.Should().BeSameAs(prototype);
        }

        private void ThenExpectNoDeclaration()
            => ResultDeclaration.Should().BeNull();
    }
}