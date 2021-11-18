using System;

using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public class CodeNameTests
    {
        // --------
        // Creation
        // --------

        // Valid names

        [Fact]
        public void CodeNameFrom_ShouldAcceptLettersName()
        {
            var name = CodeName.From("Lorem");
            name.Value.Should().Be("Lorem");
        }

        [Fact]
        public void CodeNameFrom_ShouldAcceptAlphanumericName()
        {
            var name = CodeName.From("FarOutPost42");
            name.Value.Should().Be("FarOutPost42");
        }

        [Fact]
        public void CodeNameFrom_ShouldAcceptNameWithUnderscores()
        {
            var name = CodeName.From("snake_case");
            name.Value.Should().Be("snake_case");
        }

        [Fact]
        public void CodeNameFrom_ShouldAcceptSingleUnderscoreName()
        {
            var name = CodeName.From("_");
            name.Value.Should().Be("_");
        }

        // Invalid names

        [Fact]
        public void CodeNameFrom_ShouldDisallowSpecialSymbols()
        {
            Action creationAction = () => CodeName.From("question_mark?");
            creationAction.Should().ThrowExactly<ArgumentException>()
                .Which.ParamName.Should().Be("name");
        }

        [Fact]
        public void CodeNameFrom_ShouldDisallowNumbersAtStart()
        {
            Action creationAction = () => CodeName.From("123audio_test");
            creationAction.Should().ThrowExactly<ArgumentException>()
                .Which.ParamName.Should().Be("name");
        }

        // --------
        // Equality
        // --------

        [Fact]
        public void CodeName_ShouldNotEqualItsValue()
        {
            var name = CodeName.From("test_name");
            name.Should().NotBe("test_name");
        }

        [Fact]
        public void CodeName_ShouldEqualOtherNameWithSameValue()
        {
            var firstName = CodeName.From("test_name");
            var secondName = CodeName.From("test_name");
            firstName.Should().Be(secondName);
            firstName.GetHashCode().Should().Be(secondName.GetHashCode());
            (firstName == secondName).Should().BeTrue();
        }

        [Fact]
        public void CodeName_ShouldEqualOtherNameWithDifferentCase()
        {
            var firstName = CodeName.From("test_name");
            var secondName = CodeName.From("TEST_NAME");
            firstName.Should().Be(secondName);
            firstName.GetHashCode().Should().Be(secondName.GetHashCode());
            (firstName == secondName).Should().BeTrue();
        }

        [Fact]
        public void CodeName_ShouldNotEqualNameWithDifferentValue()
        {
            var firstName = CodeName.From("test_name");
            var secondName = CodeName.From("more_test_name");
            firstName.Should().NotBe(secondName);
            (firstName != secondName).Should().BeTrue();
        }
    }
}
