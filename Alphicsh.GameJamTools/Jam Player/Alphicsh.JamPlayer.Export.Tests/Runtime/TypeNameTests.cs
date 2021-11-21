using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public class TypeNameTests
    {
        // --------
        // Creation
        // --------

        // Simple type names

        [Fact]
        public void TypeNameCreateSimple_ShouldAcceptValidString()
        {
            var typeName = TypeName.CreateSimple("Type_name_1");
            
            typeName.RootName.Should().Be(CodeName.From("Type_name_1"));
            typeName.GenericArguments.Should().BeEmpty();
        }

        [Fact]
        public void TypeNameCreateSimple_ShouldDisallowInvalidString()
        {
            Action creationAction = () => TypeName.CreateSimple("question_mark?");
            
            creationAction.Should().Throw<Exception>(); // it's up to CodeName to decide which exception to throw
        }

        [Fact]
        public void TypeNameCreateSimple_ShouldAcceptValidCodeName()
        {
            var codeName = CodeName.From("Type_name_1");
            var typeName = TypeName.CreateSimple(codeName);
            
            typeName.RootName.Should().Be(codeName);
            typeName.GenericArguments.Should().BeEmpty();
        }
        
        // Generic type names

        [Fact]
        public void TypeNameCreateGeneric_ShouldAcceptSingleArgument()
        {
            var firstArgumentName = TypeName.CreateSimple("Lorem");
            var typeName = TypeName.CreateGeneric("TestType", firstArgumentName);
            
            typeName.RootName.Should().Be(CodeName.From("TestType"));
            typeName.GenericArguments.Should().ContainInOrder(firstArgumentName);
        }

        [Fact]
        public void TypeNameCreateGeneric_ShouldAcceptMultipleArguments()
        {
            var firstArgumentName = TypeName.CreateSimple("Lorem");
            var secondArgumentName = TypeName.CreateSimple("Ipsum");
            var typeName = TypeName.CreateGeneric("TestType", firstArgumentName, secondArgumentName);
            
            typeName.RootName.Should().Be(CodeName.From("TestType"));
            typeName.GenericArguments.Should().ContainInOrder(firstArgumentName, secondArgumentName);
        }

        [Fact]
        public void TypeNameCreateGeneric_ShouldAcceptNestedArguments()
        {
            var nestedTypeName = TypeName.CreateSimple("Nested");
            var wrapperTypeName = TypeName.CreateGeneric("Wrapper", nestedTypeName);
            var typeName = TypeName.CreateGeneric("TestType", wrapperTypeName);
            
            typeName.RootName.Should().Be(CodeName.From("TestType"));
            typeName.GenericArguments.Should().ContainInOrder(wrapperTypeName);
        }
        
        [Fact]
        public void TypeNameCreateGeneric_ShouldWorkWithCodeNameAndParamsOverload()
        {
            var firstArgumentName = TypeName.CreateSimple("Lorem");
            var typeName = TypeName.CreateGeneric(CodeName.From("TestType"), firstArgumentName);
            
            typeName.RootName.Should().Be(CodeName.From("TestType"));
            typeName.GenericArguments.Should().ContainInOrder(firstArgumentName);
        }
        
        [Fact]
        public void TypeNameCreateGeneric_ShouldWorkWithStringAndEnumerableOverload()
        {
            var firstArgumentName = TypeName.CreateSimple("Lorem");
            var typeName = TypeName.CreateGeneric("TestType", new List<TypeName> { firstArgumentName });
            
            typeName.RootName.Should().Be(CodeName.From("TestType"));
            typeName.GenericArguments.Should().ContainInOrder(firstArgumentName);
        }
        
        // --------
        // ToString
        // --------
        
        [Fact]
        public void TypeNameToString_ShouldEqualItsRootForSimpleType()
        {
            var typeName = TypeName.CreateSimple("Type_name_1");
            
            typeName.ToString().Should().Be("Type_name_1");
        }

        [Fact]
        public void TypeNameToString_ShouldIncludeSingleGenericArgument()
        {
            var firstArgumentName = TypeName.CreateSimple("Lorem");
            var typeName = TypeName.CreateGeneric("TestType", firstArgumentName);
            
            typeName.ToString().Should().Be("TestType<Lorem>");
        }

        [Fact]
        public void TypeNameToString_ShouldIncludeMultipleGenericArguments()
        {
            var firstArgumentName = TypeName.CreateSimple("Lorem");
            var secondArgumentName = TypeName.CreateSimple("Ipsum");
            var typeName = TypeName.CreateGeneric("TestType", firstArgumentName, secondArgumentName);
            
            typeName.ToString().Should().Be("TestType<Lorem,Ipsum>");
        }

        [Fact]
        public void TypeNameToString_ShouldIncludeNestedGenericArguments()
        {
            var nestedTypeName = TypeName.CreateSimple("Nested");
            var wrapperTypeName = TypeName.CreateGeneric("Wrapper", nestedTypeName);
            var typeName = TypeName.CreateGeneric("TestType", wrapperTypeName);
            
            typeName.ToString().Should().Be("TestType<Wrapper<Nested>>");
        }

        // --------
        // Equality
        // --------

        [Fact]
        public void SimpleTypeName_ShouldEqualOtherSimpleTypeNameWithSameRoot()
        {
            var firstName = TypeName.CreateSimple("TestType");
            var secondName = TypeName.CreateSimple("TestType");

            firstName.Should().Be(secondName);
            firstName.GetHashCode().Should().Be(secondName.GetHashCode());
            (firstName == secondName).Should().BeTrue();
        }

        [Fact]
        public void SimpleTypeName_ShouldNotEqualOtherSimpleTypeNameWithDifferentRoot()
        {
            var firstName = TypeName.CreateSimple("TestType");
            var secondName = TypeName.CreateSimple("OtherTestType");

            firstName.Should().NotBe(secondName);
            (firstName != secondName).Should().BeTrue();
        }

        [Fact]
        public void GenericTypeName_ShouldEqualOtherGenericTypeNameWithSameRootAndArguments()
        {
            var firstArgument = TypeName.CreateSimple("Lorem");
            var secondArgument = TypeName.CreateSimple("Ipsum");
            var firstName = TypeName.CreateGeneric("TestType", firstArgument, secondArgument);
            var secondName = TypeName.CreateGeneric("TestType", firstArgument, secondArgument);
            
            firstName.Should().Be(secondName);
            firstName.GetHashCode().Should().Be(secondName.GetHashCode());
            (firstName == secondName).Should().BeTrue();
        }

        [Fact]
        public void GenericTypeName_ShouldNotEqualOtherGenericTypeNameWithDifferentRoot()
        {
            var firstArgument = TypeName.CreateSimple("Lorem");
            var secondArgument = TypeName.CreateSimple("Ipsum");
            var firstName = TypeName.CreateGeneric("TestType", firstArgument, secondArgument);
            var secondName = TypeName.CreateGeneric("OtherTestType", firstArgument, secondArgument);
            
            firstName.Should().NotBe(secondName);
            (firstName != secondName).Should().BeTrue();
        }

        [Fact]
        public void GenericTypeName_ShouldNotEqualOtherGenericTypeNameWithDifferentArguments()
        {
            var firstArgument = TypeName.CreateSimple("Lorem");
            var secondArgument = TypeName.CreateSimple("Ipsum");
            var firstName = TypeName.CreateGeneric("TestType", firstArgument);
            var secondName = TypeName.CreateGeneric("TestType", secondArgument);
            
            firstName.Should().NotBe(secondName);
            (firstName != secondName).Should().BeTrue();
        }
    }
}