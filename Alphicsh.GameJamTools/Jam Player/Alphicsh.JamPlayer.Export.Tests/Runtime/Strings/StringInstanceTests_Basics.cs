using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Strings
{
    public partial class StringInstanceTests
    {
        [Fact]
        public void Length_ShouldBeStringLength()
        {
            GivenStringInstanceFrom("12345");
            
            WhenAccessingGetter("length");
            
            ThenExpectNumberGetterValue(5);
        }

        [Fact]
        public void Length_ShouldBeZeroForEmptyString()
        {
            GivenStringInstanceFrom("");
            
            WhenAccessingGetter("length");
            
            ThenExpectNumberGetterValue(0);
        }
    }
}