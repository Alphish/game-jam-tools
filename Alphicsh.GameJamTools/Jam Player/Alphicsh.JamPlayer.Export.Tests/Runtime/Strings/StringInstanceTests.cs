using System;
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
        
        private void ThenExpectStringMethodResult(string innerString)
        {
            ThenMethodResult().Should().BeOfType<StringInstance>()
                .Which.InnerString.Should().Be(innerString);
        }
    }
}