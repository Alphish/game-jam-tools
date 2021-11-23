using System;
using Alphicsh.JamPlayer.Export.Runtime.Numbers;
using FluentAssertions;

namespace Alphicsh.JamPlayer.Export.Runtime.Strings
{
    public partial class StringInstanceTests : InstanceTestBase<StringInstance>
    {
        private void GivenStringInstanceFrom(string innerString)
        {
            var instance = new StringInstance(innerString);
            GivenInstance(instance);
        }
        
        // -------------
        // Getter values
        // -------------
        
        private void ThenExpectNumberGetterValue(double innerValue)
        {
            ThenGetterValue().Should().BeOfType<NumberInstance>()
                .Which.InnerValue.Should().Be(innerValue);
        }

        // --------------
        // Method results
        // --------------
        
        private void ThenExpectStringMethodResult(string innerString)
        {
            ThenMethodResult().Should().BeOfType<StringInstance>()
                .Which.InnerString.Should().Be(innerString);
        }
    }
}