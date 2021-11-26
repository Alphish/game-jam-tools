using System;
using Alphicsh.JamPlayer.Export.Parser.Scanning.Exceptions;
using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Parser.Scanning
{
    public class ExportScriptScannerTests
    {
        // ---------------
        // Simple scanning
        // ---------------
        
        [Fact]
        public void ScanContent_ShouldYieldNoTokensForEmptyString()
        {
            GivenContent(string.Empty);
            
            WhenContentParsed();
            
            ThenExpectNoMoreTokens();
        }
        
        [Theory]
        [InlineData("     \t\t\t\t    ")]
        [InlineData("\r\r\n\n\r\r\n\n")]
        [InlineData("// this is some line comment")]
        [InlineData("/* this is some block comment */")]
        public void ScanContent_ShouldYieldNoTokensForWhitespaceAndComments(string content)
        {
            GivenContent(content);
            
            WhenContentParsed();
            
            ThenExpectNoMoreTokens();
        }
        
        [Theory]
        [InlineData("lorem")]
        [InlineData("12345")]
        [InlineData("with_underscore")]
        [InlineData("123_sound_test")]
        public void ScanContent_ShouldYieldWordTokenForSingleWord(string word)
        {
            GivenContent(word);
            
            WhenContentParsed();
            
            ThenExpectWord(word);
            ThenExpectNoMoreTokens();
        }
        
        [Theory]
        [InlineData(".")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        [InlineData("/")]
        [InlineData("%")]
        public void ScanContent_ShouldYieldSymbolTokenForSingleSymbol(string symbol)
        {
            GivenContent(symbol);
            
            WhenContentParsed();
            
            ThenExpectSymbol(symbol);
            ThenExpectNoMoreTokens();
        }
        
        [Theory]
        [InlineData("\"Hello, world!\"", "Hello, world!")]
        [InlineData("'apostrophe wrap'", "apostrophe wrap")]
        [InlineData("\"Escaped \\\"quote\\\"\"", "Escaped \"quote\"")]
        [InlineData("'Escaped \\'apostrophe\\''", "Escaped 'apostrophe'")]
        [InlineData("\"Words\\tTabs\\nNewlines\\rCarriage returns\"", "Words\tTabs\nNewlines\rCarriage returns")]
        [InlineData("'C:\\\\File\\\\Path'", "C:\\File\\Path")]
        public void ScanContent_ShouldYieldSymbolTokenForSingleString(string parsedContent, string stringContent)
        {
            GivenContent(parsedContent);
            
            WhenContentParsed();
            
            ThenExpectString(stringContent);
            ThenExpectNoMoreTokens();
        }

        [Fact]
        public void ScanContent_ShouldYieldMultipleTokensInExpression()
        {
            GivenContent("'Overall: ' + overall");
            
            WhenContentParsed();
            
            ThenExpectString("Overall: ");
            ThenExpectSymbol("+");
            ThenExpectWord("overall");
            ThenExpectNoMoreTokens();
        }
        
        // ---------------
        // Errors handling
        // ---------------

        [Fact]
        public void ScanContent_ShouldThrowForUnexpectedAsciiCharacter()
        {
            GivenContent("It crashes \b");
            
            WhenContentParseAttempted();
            
            ThenExpectUnexpectedCharacterException('\b');
        }
        
        [Fact]
        public void ScanContent_ShouldThrowForUnexpectedNonAsciiCharacter()
        {
            GivenContent("It crashes ಠ_ಠ");
            
            WhenContentParseAttempted();
            
            ThenExpectUnexpectedCharacterException('ಠ');
        }
        
        [Fact]
        public void ScanContent_ShouldThrowForUnmatchedBlockComment()
        {
            GivenContent("/* did I forget about anything?");
            
            WhenContentParseAttempted();
            
            ThenExpectUnmatchedBlockCommentException();
        }

        [Fact]
        public void ScanContent_ShouldThrowForUnmatchedStringBeforeNewline()
        {
            GivenContent("'this is some string\nwith newlines'");
            
            WhenContentParseAttempted();
            
            ThenExpectUnmatchedStringException();
        }

        [Fact]
        public void ScanContent_ShouldThrowForUnmatchedStringBeforeEnd()
        {
            GivenContent("'this is some string...");
            
            WhenContentParseAttempted();
            
            ThenExpectUnmatchedStringException();
        }

        [Fact]
        public void ScanContent_ShouldThrowForUnknownEscapeSequence()
        {
            GivenContent("'press \\F to pay respects'");
            
            WhenContentParseAttempted();
            
            ThenExpectUnknownEscapeSequenceException("\\F");
        }

        // -----
        // Setup
        // -----

        private ExportScriptScanner Scanner = new ExportScriptScanner();
        private string Content { get; set; } = default!;

        private ExportScriptTokenSequence? TokenSequence { get; set; }
        private Action? ContentParsingAttempt { get; set; }
        
        // Givens

        private void GivenContent(string content)
            => Content = content;
        
        // Whens

        private void WhenContentParsed()
            => TokenSequence = Scanner.ScanContent(Content);
        
        private void WhenContentParseAttempted()
            => ContentParsingAttempt = () => TokenSequence = Scanner.ScanContent(Content);
        
        // Thens

        private void ThenExpectWord(string content)
            => TokenSequence!.ExpectWord(content);
        private void ThenExpectSymbol(string content)
            => TokenSequence!.ExpectSymbol(content);
        private void ThenExpectString(string content)
            => TokenSequence!.ExpectString(content);
        private void ThenExpectNoMoreTokens()
            => TokenSequence!.ExpectEndOfTokens();

        private void ThenExpectUnexpectedCharacterException(char character)
        {
            ContentParsingAttempt.Should().ThrowExactly<UnexpectedCharacterException>()
                .Which.Character.Should().Be(character);
        }
        
        private void ThenExpectUnknownEscapeSequenceException(string sequence)
        {
            ContentParsingAttempt.Should().ThrowExactly<UnknownEscapeSequenceException>()
                .Which.Sequence.Should().Be(sequence);
        }

        private void ThenExpectUnmatchedBlockCommentException()
            => ContentParsingAttempt.Should().ThrowExactly<UnmatchedBlockCommentException>();

        private void ThenExpectUnmatchedStringException()
            => ContentParsingAttempt.Should().ThrowExactly<UnmatchedStringException>();
    }
}