using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionParameterListTests
    {
        // -------------
        // List creation
        // -------------
        
        [Fact]
        public void FunctionParameterList_ShouldBeCreatedWithoutParameters()
        {
            GivenNoParameters();
            WhenParameterListCreated();
            ThenExpectValidParameterList();
        }
        
        [Fact]
        public void FunctionParameterList_ShouldBeCreatedWithGivenParameters()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Ipsum);
            WhenParameterListCreated();
            ThenExpectValidParameterList();
        }

        [Fact]
        public void FunctionParameterList_ShouldAllowMultipleParametersOfSameType()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Lorem);
            GivenParameter("ccc", SimpleTestPrototype.Lorem);
            WhenParameterListCreated();
            ThenExpectValidParameterList();
        }

        [Fact]
        public void FunctionParameterList_ShouldDisallowDuplicateParameterNames()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            WhenParameterListCreationAttempted();
            ThenExpectArgumentException("parameters");
        }
        
        // -----------------
        // Argument matching
        // -----------------

        [Fact]
        public void ParameterListMatch_ShouldMatchNoParametersWithNoArguments()
        {
            GivenNoParameters();
            GivenNoArguments();
            WhenArgumentTypesMatched();
            ThenExpectTypesMatch();
        }

        [Fact]
        public void ParameterListMatch_ShouldNotMatchNoParametersWithSomeArguments()
        {
            GivenNoParameters();
            GivenArgumentType(SimpleTestPrototype.Lorem);
            WhenArgumentTypesMatched();
            ThenExpectTypesMismatch();
        }

        [Fact]
        public void ParameterListMatch_ShouldMatchSomeParametersWithSameTypeArguments()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Ipsum);
            GivenArgumentType(SimpleTestPrototype.Lorem);
            GivenArgumentType(SimpleTestPrototype.Ipsum);
            WhenArgumentTypesMatched();
            ThenExpectTypesMatch();
        }

        [Fact]
        public void ParameterListMatch_ShouldNotMatchSomeParametersWithDifferentTypeArguments()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Ipsum);
            GivenArgumentType(SimpleTestPrototype.Lorem);
            GivenArgumentType(SimpleTestPrototype.Lorem);
            WhenArgumentTypesMatched();
            ThenExpectTypesMismatch();
        }

        [Fact]
        public void ParameterListMatch_ShouldNotMatchWithMissingArguments()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Ipsum);
            GivenArgumentType(SimpleTestPrototype.Lorem);
            WhenArgumentTypesMatched();
            ThenExpectTypesMismatch();
        }

        [Fact]
        public void ParameterListMatch_ShouldNotMatchWithExtraArguments()
        {
            GivenParameter("aaa", SimpleTestPrototype.Lorem);
            GivenParameter("bbb", SimpleTestPrototype.Ipsum);
            GivenArgumentType(SimpleTestPrototype.Lorem);
            GivenArgumentType(SimpleTestPrototype.Ipsum);
            GivenArgumentType(SimpleTestPrototype.Ipsum);
            WhenArgumentTypesMatched();
            ThenExpectTypesMismatch();
        }

        [Fact]
        public void ParameterListMatch_ShouldMatchSubtypeArgumentWithSupertypeParameter()
        {
            GivenParameter("aaa", SimpleTestPrototype.Ipsum);
            GivenArgumentType(SimpleTestPrototype.SubIpsum);
            WhenArgumentTypesMatched();
            ThenExpectTypesMatch();
        }

        [Fact]
        public void ParameterListMatch_ShouldNotMatchSupertypeArgumentWithSubtypeParameter()
        {
            GivenParameter("aaa", SimpleTestPrototype.SubIpsum);
            GivenArgumentType(SimpleTestPrototype.Ipsum);
            WhenArgumentTypesMatched();
            ThenExpectTypesMismatch();
        }

        // --------------
        // Helper methods
        // --------------

        private ICollection<VariableDeclaration> Parameters { get; } = new List<VariableDeclaration>();
        private ICollection<IPrototype> ArgumentTypes { get; } = new List<IPrototype>();
        
        private Action? ParameterListCreationAttempt { get; set; }
        private FunctionParameterList? ParameterList { get; set; }
        private bool? AreArgumentsMatched { get; set; }
        
        // Givens

        private void GivenNoParameters()
        {
        }

        private void GivenParameter(string name, IPrototype prototype)
        {
            Parameters.Add(VariableDeclaration.Create(name, prototype));
        }

        private void GivenNoArguments()
        {
        }

        private void GivenArgumentType(IPrototype argumentType)
        {
            ArgumentTypes.Add(argumentType);
        }
        
        // Whens

        private void WhenParameterListCreated()
        {
            ParameterList = new FunctionParameterList(Parameters);
        }

        private void WhenParameterListCreationAttempted()
        {
            ParameterListCreationAttempt = WhenParameterListCreated;
        }

        private void WhenArgumentTypesMatched()
        {
            ParameterList = new FunctionParameterList(Parameters);
            AreArgumentsMatched = ParameterList.MatchesArgumentTypes(ArgumentTypes);
        }
        
        // Thens

        private void ThenExpectValidParameterList()
        {
            ParameterList.Should().NotBeNull();
            ParameterList!.Count.Should().Be(Parameters.Count);

            // looping through IEnumerable version like a silly person
            // so that I can get coverage on IEnumerable.GetEnumerator
            // which - considering I'm trying to get it all covered - might actually make me a silly person
            // oh well
            var idx = 0;
            foreach (var item in (IEnumerable)ParameterList)
            {
                item.Should().Be(Parameters.ElementAt(idx++));
            }
        }

        private void ThenExpectArgumentException(string expectedParamName)
        {
            ParameterListCreationAttempt.Should().ThrowExactly<ArgumentException>()
                .Which.ParamName.Should().Be(expectedParamName);
        }

        private void ThenExpectTypesMatch()
        {
            AreArgumentsMatched.Should().BeTrue();
        }
        
        private void ThenExpectTypesMismatch()
        {
            AreArgumentsMatched.Should().BeFalse();
        }
    }
}