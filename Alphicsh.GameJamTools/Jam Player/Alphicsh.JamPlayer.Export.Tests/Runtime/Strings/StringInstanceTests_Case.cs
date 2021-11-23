using System.Linq;

using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Runtime.Strings
{
    public partial class StringInstanceTests
    {
        [Fact]
        public void ToUpper_ShouldReturnUppercaseString()
        {
            GivenStringInstanceFrom("Test String 123...");
            GivenMethodName("toUpper");
            GivenNoMethodArguments();

            WhenMethodExecuted();
            
            ThenExpectStringMethodResult("TEST STRING 123...");
        }
        
        [Fact]
        public void ToLower_ShouldReturnLowercaseString()
        {
            GivenStringInstanceFrom("Test String 123...");
            GivenMethodName("toLower");
            GivenNoMethodArguments();

            WhenMethodExecuted();
            
            ThenExpectStringMethodResult("test string 123...");
        }
    }
}