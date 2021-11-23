using System;
using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class ConstantExpressionTests
    {
        [Fact]
        public void ConstantExpression_CanBeCreatedForDeclaredTypeInstance()
        {
            var prototype = SimpleTestPrototype.Lorem;
            var instance = SimpleTestPrototype.Lorem.CreateInstance();
            var expression = ConstantExpression.Create(prototype, instance);
            var variableScope = new VariableScope();

            expression.EvaluatedType.Should().BeSameAs(prototype);
            expression.Evaluate(variableScope).Should().BeSameAs(instance);
        }
        
        [Fact]
        public void ConstantExpression_CannotBeCreatedForDifferentTypeInstance()
        {
            var prototype = SimpleTestPrototype.Lorem;
            var instance = SimpleTestPrototype.Ipsum.CreateInstance();
            Action expressionCreationAttempt = () =>
            {
                var expression = ConstantExpression.Create(prototype, instance);
            };

            expressionCreationAttempt.Should().Throw<ExpressionCompilationException>();
        }
        
        [Fact]
        public void ConstantExpression_CanBeCreatedForSubtypeOfDeclaredTypeInstance()
        {
            var prototype = SimpleTestPrototype.Ipsum;
            var instance = SimpleTestPrototype.SubIpsum.CreateInstance();
            var expression = ConstantExpression.Create(prototype, instance);
            var variableScope = new VariableScope();

            expression.EvaluatedType.Should().BeSameAs(prototype);
            expression.Evaluate(variableScope).Should().BeSameAs(instance);
        }
    }
}