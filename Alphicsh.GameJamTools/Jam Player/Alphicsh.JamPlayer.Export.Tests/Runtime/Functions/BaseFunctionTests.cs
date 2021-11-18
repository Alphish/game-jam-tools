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
            GivenReturnType(FunctionTestPrototype.Lorem);
            GivenNoArguments();
            GivenExpectedResult(FunctionTestPrototype.Lorem.CreateInstance());
            
            WhenFunctionExecuted();
            
            ThenExpectValidResult();
        }
        
        [Fact]
        public void FunctionCall_ShouldBeValidForFunctionWithParameters()
        {
            GivenParameter("aaa", FunctionTestPrototype.Lorem);
            GivenParameter("bbb", FunctionTestPrototype.Lorem);
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenExpectedResult(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecuted();
            
            ThenExpectValidResult();
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowArgumentExceptionForTooFewArguments()
        {
            GivenParameter("aaa", FunctionTestPrototype.Lorem);
            GivenParameter("bbb", FunctionTestPrototype.Lorem);
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenExpectedResult(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectArgumentException("arguments");
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowArgumentExceptionForTooManyArguments()
        {
            GivenParameter("aaa", FunctionTestPrototype.Lorem);
            GivenParameter("bbb", FunctionTestPrototype.Lorem);
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenExpectedResult(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectArgumentException("arguments");
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowArgumentExceptionForDifferentTypeArguments()
        {
            GivenParameter("aaa", FunctionTestPrototype.Lorem);
            GivenParameter("bbb", FunctionTestPrototype.Lorem);
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenArgument(FunctionTestPrototype.Ipsum.CreateInstance());
            GivenExpectedResult(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectArgumentException("arguments");
        }
        
        [Fact]
        public void FunctionCall_ShouldBeValidForSubtypeArgument()
        {
            GivenParameter("aaa", FunctionTestPrototype.Lorem);
            GivenParameter("bbb", FunctionTestPrototype.Ipsum);
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenArgument(FunctionTestPrototype.SubIpsum.CreateInstance());
            GivenExpectedResult(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecuted();
            
            ThenExpectValidResult();
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowArgumentExceptionForSupertypeArgument()
        {
            GivenParameter("aaa", FunctionTestPrototype.Lorem);
            GivenParameter("bbb", FunctionTestPrototype.SubIpsum);
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenArgument(FunctionTestPrototype.Lorem.CreateInstance());
            GivenArgument(FunctionTestPrototype.Ipsum.CreateInstance());
            GivenExpectedResult(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectArgumentException("arguments");
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowInvalidCastExceptionForMismatchedReturn()
        {
            GivenNoParameters();
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenNoArguments();
            GivenExpectedResult(FunctionTestPrototype.Lorem.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectInvalidCastException();
        }
        
        [Fact]
        public void FunctionCall_ShouldBeValidForSubtypeReturn()
        {
            GivenNoParameters();
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenNoArguments();
            GivenExpectedResult(FunctionTestPrototype.SubIpsum.CreateInstance());
            
            WhenFunctionExecuted();
            
            ThenExpectValidResult();
        }
        
        [Fact]
        public void FunctionCall_ShouldThrowInvalidCastExceptionForSupertypeReturn()
        {
            GivenNoParameters();
            GivenReturnType(FunctionTestPrototype.SubIpsum);
            GivenNoArguments();
            GivenExpectedResult(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenFunctionExecutionAttempted();
            
            ThenExpectInvalidCastException();
        }
        
        // --------------
        // Helper methods
        // --------------

        private ICollection<FunctionParameter> Parameters { get; } = new List<FunctionParameter>();
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
            Parameters.Add(FunctionParameter.Create(name, prototype));
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