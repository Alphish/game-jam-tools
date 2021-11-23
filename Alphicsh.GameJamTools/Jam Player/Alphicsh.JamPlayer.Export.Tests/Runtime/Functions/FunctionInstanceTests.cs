using System;
using System.Linq;

using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionInstanceTests
    {
        // --------------------
        // Retrieving prototype
        // --------------------
        
        [Fact]
        public void FunctionPrototypeMatchingFunction_ShouldCreatePrototypeForFunctionWithoutParameters()
        {
            var function = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                argumentTypes: new IPrototype[0]
            );
            var prototype = FunctionPrototype.MatchingFunction(function);

            prototype.Name.ToString().Should().Be("Function<Lorem>");
        }
        
        [Fact]
        public void FunctionPrototypeMatchingFunction_ShouldCreatePrototypeForFunctionWithParameters()
        {
            var function = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var prototype = FunctionPrototype.MatchingFunction(function);

            prototype.Name.ToString().Should().Be("Function<Ipsum,Ipsum,Lorem>");
        }
        
        [Fact]
        public void FunctionPrototypeMatchingFunction_ShouldProvideSamePrototypeForSameParameterAndReturnTypes()
        {
            var firstFunction = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var firstPrototype = FunctionPrototype.MatchingFunction(firstFunction);

            var secondFunction = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var secondPrototype = FunctionPrototype.MatchingFunction(secondFunction);

            firstFunction.Should().NotBeSameAs(secondFunction);
            firstPrototype.Should().BeSameAs(secondPrototype);
        }
        
        [Fact]
        public void FunctionPrototypeMatchingMethod_ShouldCreatePrototypeForMethodWithoutParameters()
        {
            var unboundMethod = CreateTestMethod(
                returnType: SimpleTestPrototype.Lorem,
                argumentTypes: new IPrototype[0]
            );
            var prototype = FunctionPrototype.MatchingMethod(unboundMethod);

            prototype.Name.ToString().Should().Be("Function<Lorem>");
        }
        
        [Fact]
        public void FunctionPrototypeMatchingMethod_ShouldCreatePrototypeForMethodWithParameters()
        {
            var unboundMethod = CreateTestMethod(
                returnType: SimpleTestPrototype.Lorem,
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var prototype = FunctionPrototype.MatchingMethod(unboundMethod);

            prototype.Name.ToString().Should().Be("Function<Ipsum,Ipsum,Lorem>");
        }
        
        // ----------------
        // Call return type
        // ----------------

        [Fact]
        public void GetCallReturnType_ShouldGiveTypeForSameArgumentTypes()
        {
            var function = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var prototype = FunctionPrototype.MatchingFunction(function);

            var returnType = prototype.GetCallReturnType(new[]
            {
                SimpleTestPrototype.Ipsum,
                SimpleTestPrototype.Ipsum,
            });

            returnType.Should().Be(SimpleTestPrototype.Lorem);
        }
        
        [Fact]
        public void GetCallReturnType_ShouldGiveTypeForMatchingArgumentTypes()
        {
            var function = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var prototype = FunctionPrototype.MatchingFunction(function);

            var returnType = prototype.GetCallReturnType(new[]
            {
                SimpleTestPrototype.SubIpsum,
                SimpleTestPrototype.Ipsum,
            });

            returnType.Should().Be(SimpleTestPrototype.Lorem);
        }
        
        [Fact]
        public void GetCallReturnType_ShouldReturnNullForDifferentArgumentTypes()
        {
            var function = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var prototype = FunctionPrototype.MatchingFunction(function);

            var returnType = prototype.GetCallReturnType(new[]
            {
                SimpleTestPrototype.Lorem,
                SimpleTestPrototype.Ipsum,
            });

            returnType.Should().BeNull();
        }
        
        [Fact]
        public void GetCallReturnType_ShouldReturnNullForMissingArgumentTypes()
        {
            var function = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var prototype = FunctionPrototype.MatchingFunction(function);

            var returnType = prototype.GetCallReturnType(new[]
            {
                SimpleTestPrototype.Ipsum,
            });

            returnType.Should().BeNull();
        }
        
        [Fact]
        public void GetCallReturnType_ShouldReturnNullForExtraArgumentTypes()
        {
            var function = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var prototype = FunctionPrototype.MatchingFunction(function);

            var returnType = prototype.GetCallReturnType(new[]
            {
                SimpleTestPrototype.Ipsum,
                SimpleTestPrototype.Ipsum,
                SimpleTestPrototype.Ipsum,
            });

            returnType.Should().BeNull();
        }
        
        // ----
        // Call
        // ----

        [Fact]
        public void Call_ShouldReturnInnerFunctionResult()
        {
            var function = CreateTestFunction(
                result: SimpleTestPrototype.Lorem.CreateInstance(),
                SimpleTestPrototype.Ipsum,    // first argument type
                SimpleTestPrototype.Ipsum     // second argument type
            );
            var instance = function.ToFunctionInstance();
            var arguments = new[]
            {
                SimpleTestPrototype.Ipsum.CreateInstance(),
                SimpleTestPrototype.Ipsum.CreateInstance(),
            };

            var functionCallResult = function.Call(arguments);
            var instanceCallResult = instance.Call(arguments);

            instanceCallResult.Should().BeSameAs(functionCallResult);
        }
        
        // --------------
        // Helper methods
        // --------------

        private IFunction CreateTestFunction(IInstance result, params IPrototype[] argumentTypes)
        {
            var parameterList = argumentTypes
                .Select((type, i) => VariableDeclaration.Create("arg" + i, type))
                .ToList();
            var returnType = result.Prototype;

            return new MockFunction(parameterList, returnType, result);
        }
        
        private IUnboundMethod CreateTestMethod(IPrototype returnType, params IPrototype[] argumentTypes)
        {
            var parameterList = argumentTypes
                .Select((type, i) => VariableDeclaration.Create("arg" + i, type))
                .ToList();
            InstanceMethodCallback callback = (instance, arguments) => throw new NotImplementedException();

            return new UnboundMethod(parameterList, returnType, callback);
        }
    }
}