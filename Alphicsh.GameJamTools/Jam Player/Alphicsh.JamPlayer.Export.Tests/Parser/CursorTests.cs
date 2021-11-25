using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Parser
{
    public class CursorTests
    {
        [Fact]
        public void AdvanceColumns_ShouldAdvancePositionAndColumn()
        {
            var cursor = new Cursor(11, 2, 3).AdvanceColumns(10);
            cursor.Should().Be(new Cursor(21, 2, 13));
        }
        
        [Fact]
        public void AdvanceColumns_ShouldAdvancePositionAndLineAndResetColumn()
        {
            var cursor = new Cursor(11, 2, 3).AdvanceLine();
            cursor.Should().Be(new Cursor(12, 3, 0));
        }
        
        [Fact]
        public void ToString_ShouldReturnOneIndexedLineColumnAndPosition()
        {
            var cursor = new Cursor(1234, 56, 78);
            cursor.ToString()
                .Should().Contain(1235.ToString())
                .And.Contain(57.ToString())
                .And.Contain(79.ToString());
        }
        
        [Fact]
        public void ToShortString_ShouldReturnOneIndexedLineAndColumnButNotPosition()
        {
            var cursor = new Cursor(1234, 56, 78);
            cursor.ToShortString()
                .Should().NotContain(1235.ToString())
                .And.Contain(57.ToString())
                .And.Contain(79.ToString());
        }
    }
}