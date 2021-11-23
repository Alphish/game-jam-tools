using System;
using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class VariableExpressionTests
    {
        [Fact]
        public void VariableExpression_CanBeCreatedForKnownVariable()
        {
            var declarationScope = new VariableDeclarationScope();
            var declaration = VariableDeclaration.Create("test", SimpleTestPrototype.Lorem);
            declarationScope.DeclareVariable(declaration);
            var expression = VariableExpression.Create(declarationScope, CodeName.From("test"));

            expression.Should().Be(expression); // deep, I know; the point is that expression exists in the first place
        }
        
        [Fact]
        public void VariableExpression_CannotBeCreatedForUnknownVariable()
        {
            var declarationScope = new VariableDeclarationScope();
            var declaration = VariableDeclaration.Create("test", SimpleTestPrototype.Lorem);
            declarationScope.DeclareVariable(declaration);
            Action expressionCreationAttempt = () =>
            {
                var expression = VariableExpression.Create(declarationScope, CodeName.From("otherTest"));
            };

            expressionCreationAttempt.Should().ThrowExactly<ExpressionCompilationException>();
        }

        [Fact]
        public void EvaluatedType_MatchesVariableDeclaration()
        {
            var declarationScope = new VariableDeclarationScope();
            var declaration = VariableDeclaration.Create("test", SimpleTestPrototype.Lorem);
            declarationScope.DeclareVariable(declaration);
            var expression = VariableExpression.Create(declarationScope, CodeName.From("test"));

            expression.EvaluatedType.Should().BeSameAs(declaration.Prototype);
        }

        [Fact]
        public void Evaluate_ShouldReturnVariableInstance()
        {
            var declarationScope = new VariableDeclarationScope();
            var declaration = VariableDeclaration.Create("test", SimpleTestPrototype.Lorem);
            declarationScope.DeclareVariable(declaration);
            var expression = VariableExpression.Create(declarationScope, CodeName.From("test"));

            var variableScope = new VariableScope();
            var instance = SimpleTestPrototype.Lorem.CreateInstance();
            variableScope.DefineVariable(declaration, instance);
            var evaluationResult = expression.Evaluate(variableScope);

            evaluationResult.Should().BeSameAs(instance);
        }
    }
}