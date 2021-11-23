using System;
using System.Collections.Generic;

using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class BaseFunctionTests
    {
        // ------------------------
        // Base function processing
        // ------------------------

        [Fact]
        public void FunctionCall_ShouldBeValidForParameterlessFunction()
        {
            GivenNoParameters();
            GivenReturnType(SimpleTestPrototype.Lorem);
            GivenNoArguments();
            GivenExpectedResult(SimpleTestPrototype.Lorem.CreateInstance());
            
            WhenFunctionExecuted();
            
            ThenExpectValidResult();
        }
        
        [Fact]
        public void FunctionCall_ShouldBeValidForFunctionWithParameters()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Lorem);
            GivenReturnType(SimpleTestPrototype.Ipsum);
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenExpectedResult(SimpleTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecuted();
            
            ThenExpectValidResult();
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowArgumentExceptionForTooFewArguments()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Lorem);
            GivenReturnType(SimpleTestPrototype.Ipsum);
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenExpectedResult(SimpleTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectArgumentException("arguments");
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowArgumentExceptionForTooManyArguments()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Lorem);
            GivenReturnType(SimpleTestPrototype.Ipsum);
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenExpectedResult(SimpleTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectArgumentException("arguments");
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowArgumentExceptionForDifferentTypeArguments()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Lorem);
            GivenReturnType(SimpleTestPrototype.Ipsum);
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenArgument(SimpleTestPrototype.Ipsum.CreateInstance());
            GivenExpectedResult(SimpleTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectArgumentException("arguments");
        }
        
        [Fact]
        public void FunctionCall_ShouldBeValidForSubtypeArgument()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Ipsum);
            GivenReturnType(SimpleTestPrototype.Ipsum);
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenArgument(SimpleTestPrototype.SubIpsum.CreateInstance());
            GivenExpectedResult(SimpleTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecuted();
            
            ThenExpectValidResult();
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowArgumentExceptionForSupertypeArgument()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.SubIpsum);
            GivenReturnType(SimpleTestPrototype.Ipsum);
            GivenArgument(SimpleTestPrototype.Lorem.CreateInstance());
            GivenArgument(SimpleTestPrototype.Ipsum.CreateInstance());
            GivenExpectedResult(SimpleTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectArgumentException("arguments");
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowInvalidCastExceptionForMismatchedReturn()
        {
            GivenNoParameters();
            GivenReturnType(SimpleTestPrototype.Ipsum);
            GivenNoArguments();
            GivenExpectedResult(SimpleTestPrototype.Lorem.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectInvalidCastException();
        }
        
        [Fact]
        public void FunctionCall_ShouldBeValidForSubtypeReturn()
        {
            GivenNoParameters();
            GivenReturnType(SimpleTestPrototype.Ipsum);
            GivenNoArguments();
            GivenExpectedResult(SimpleTestPrototype.SubIpsum.CreateInstance());
            
            WhenFunctionExecuted();
            
            ThenExpectValidResult();
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowInvalidCastExceptionForSupertypeReturn()
        {
            GivenNoParameters();
            GivenReturnType(SimpleTestPrototype.SubIpsum);
            GivenNoArguments();
            GivenExpectedResult(SimpleTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectInvalidCastException();
        }
        
        // --------------
        // Helper methods
        // --------------

        private ICollection<VariableDeclaration> Parameters { get; } = new List<VariableDeclaration>();
        private IPrototype ReturnType { get; set; } = default!;
        private ICollection<IInstance> Arguments { get; } = new List<IInstance>();
        private IInstance ExpectedResult { get; set; } = default!;
        
        private Action? FunctionExecutionAttempt { get; set; }
        private IInstance? ActualResult { get; set; }
        
        // Givens

        private void GivenNoParameters()
        {
        }

        private void GivenParameter(string name, IPrototype prototype)
        {
            Parameters.Add(VariableDeclaration.Create(name, prototype));
        }

        private void GivenReturnType(IPrototype returnType)
        {
            ReturnType = returnType;
        }

        private void GivenNoArguments()
        {
        }

        private void GivenArgument(IInstance argument)
        {
            Arguments.Add(argument);
        }

        private void GivenExpectedResult(IInstance result)
        {
            ExpectedResult = result;
        }
        
        // Whens

        private void WhenFunctionExecuted()
        {
            var function = new MockFunction(Parameters, ReturnType, ExpectedResult);
            ActualResult = function.Call(Arguments);
        }

        private void WhenFunctionExecutionAttempted()
        {
            FunctionExecutionAttempt = WhenFunctionExecuted;
        }
        
        // Thens

        private void ThenExpectValidResult()
        {
            ActualResult.Should().Be(ExpectedResult);
        }

        private void ThenExpectArgumentException(string expectedParamName)
        {
            FunctionExecutionAttempt.Should().ThrowExactly<ArgumentException>()
                .Which.ParamName.Should().Be(expectedParamName);
        }

        private void ThenExpectInvalidCastException()
        {
            FunctionExecutionAttempt.Should().ThrowExactly<InvalidCastException>();
        }
    }
}