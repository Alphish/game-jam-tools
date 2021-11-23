using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class InstanceMethodTests
    {
        [Fact]
        public void UnboundMethod_HasGivenReturnType()
        {
            GivenNoParameters();
            GivenReturnType(FunctionTestPrototype.Lorem);
            
            WhenUnboundMethodCreated();
            
            ThenExpectUnboundMethodReturnType(FunctionTestPrototype.Lorem);
        }

        [Fact]
        public void InstanceMethod_HasGivenReturnType()
        {
            GivenNoParameters();
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenBoundInstance(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenUnboundMethodCreated();
            WhenMethodBoundToInstance();

            ThenExpectInstanceMethodReturnType(FunctionTestPrototype.Ipsum);
        }

        [Fact]
        public void InstanceMethod_ShouldUseBoundInstanceInCallback()
        {
            GivenNoParameters();
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenCallback(InstanceMethodTests.GetSelf); // since it returns instance itself, it verifies the correct instance is passed
            GivenBoundInstance(FunctionTestPrototype.Ipsum.CreateInstance());
            GivenNoArguments();

            WhenUnboundMethodCreated();
            WhenMethodBoundToInstance();
            WhenInstanceMethodExecuted();

            ThenExpectMethodCallResult(Instance);
        }
        
        [Fact]
        public void InstanceMethod_ShouldUsePassedArgumentInCallback()
        {
            GivenParameter("aaa", FunctionTestPrototype.Ipsum);
            GivenReturnType(FunctionTestPrototype.Ipsum);
            GivenCallback(InstanceMethodTests.GetOnlyArgument); // since it returns the argument, it verifies the correct argument is passed
            GivenBoundInstance(FunctionTestPrototype.Lorem.CreateInstance());
            GivenArgument(FunctionTestPrototype.Ipsum.CreateInstance());
            
            WhenUnboundMethodCreated();
            WhenMethodBoundToInstance();
            WhenInstanceMethodExecuted();

            ThenExpectMethodCallResult(Arguments.Single());
        }
        
        // ---------------
        // Dummy functions
        // ---------------

        private static IInstance GetSelf(IInstance instance, IEnumerable<IInstance> arguments)
        {
            return instance;
        }

        private static IInstance GetOnlyArgument(IInstance instance, IEnumerable<IInstance> arguments)
        {
            return arguments.Single();
        }
        
        // --------------
        // Helper methods
        // --------------

        private ICollection<VariableDeclaration> Parameters { get; } = new List<VariableDeclaration>();
        private IPrototype ReturnType { get; set; } = default!;
        private InstanceMethodCallback Callback { get; set; } = default!;
        private IInstance Instance { get; set; } = default!;
        private ICollection<IInstance> Arguments { get; } = new List<IInstance>();

        private IUnboundMethod UnboundMethod { get; set; } = default!;
        private IFunction InstanceMethod { get; set; } = default!;
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

        private void GivenCallback(InstanceMethodCallback callback)
        {
            Callback = callback;
        }

        private void GivenBoundInstance(IInstance instance)
        {
            Instance = instance;
        }

        private void GivenNoArguments()
        {
        }

        private void GivenArgument(IInstance argument)
        {
            Arguments.Add(argument);
        }
        
        // Whens

        private void WhenUnboundMethodCreated()
        {
            UnboundMethod = new UnboundMethod(Parameters, ReturnType, Callback);
        }

        private void WhenMethodBoundToInstance()
        {
            InstanceMethod = UnboundMethod.Bind(Instance);
        }
        
        private void WhenInstanceMethodExecuted()
        {
            ActualResult = InstanceMethod.Call(Arguments);
        }
        
        // Thens

        private void ThenExpectUnboundMethodReturnType(IPrototype expectedReturnType)
        {
            var actualReturnType = UnboundMethod.ReturnType;
            actualReturnType.Should().Be(expectedReturnType);
        }

        private void ThenExpectInstanceMethodReturnType(IPrototype expectedReturnType)
        {
            var actualReturnType = InstanceMethod.ReturnType;
            actualReturnType.Should().Be(expectedReturnType);
        }

        private void ThenExpectMethodCallResult(IInstance expectedResult)
        {
            ActualResult.Should().Be(expectedResult);
        }
    }
}