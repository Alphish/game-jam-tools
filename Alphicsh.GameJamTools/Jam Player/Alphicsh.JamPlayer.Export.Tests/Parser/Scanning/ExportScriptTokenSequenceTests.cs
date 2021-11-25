using System;
using System.Collections;
using System.Collections.Generic;
using Alphicsh.JamPlayer.Export.Parser.Parsing.Exceptions;
using FluentAssertions;
using Xunit;

namespace Alphicsh.JamPlayer.Export.Parser.Scanning
{
    public class ExportScriptTokenSequenceTests
    {
        // ----------------
        // Basic properties
        // ----------------
        
        [Fact]
        public void ExportScriptTokenSequence_CanBeCreatedWithoutTokens()
        {
            GivenNoTokens();
            
            WhenTokenSequenceCreated();

            ThenSequence().Should().BeEmpty();
            ThenSequence().Count.Should().Be(0);
            ThenExpectNoFurtherTokens();
        }

        [Fact]
        public void ExportScriptTokenSequence_CanHandleSingleWord()
        {
            GivenToken(ExportScriptTokenType.Word, "lorem");
            
            WhenTokenSequenceCreated();

            ThenSequence().Count.Should().Be(1);
            ThenSequence().IsEndOfTokens.Should().BeFalse();

            ThenExpectCurrentToken(ExportScriptTokenType.Word, "lorem");
            ThenExpectNextToken(ExportScriptTokenType.Word, "lorem");
            ThenExpectNoFurtherTokens();
        }

        [Fact]
        public void ExportScriptTokenSequence_CanHandleMultipleTokens()
        {
            GivenToken(ExportScriptTokenType.Word, "hello");
            GivenToken(ExportScriptTokenType.String, "world");
            GivenToken(ExportScriptTokenType.Symbol, "!");
            
            WhenTokenSequenceCreated();
            
            ThenExpectCurrentToken(ExportScriptTokenType.Word, "hello");
            ThenExpectNextToken(ExportScriptTokenType.Word, "hello");
            ThenExpectCurrentToken(ExportScriptTokenType.String, "world");
            ThenExpectNextToken(ExportScriptTokenType.String, "world");
            ThenExpectCurrentToken(ExportScriptTokenType.Symbol, "!");
            ThenExpectNextToken(ExportScriptTokenType.Symbol, "!");
            ThenExpectNoFurtherTokens();
        }
        
        [Fact]
        public void ExportScriptTokenSequence_CanEnumerateTokens()
        {
            GivenToken(ExportScriptTokenType.Word, "lorem");
            
            WhenTokenSequenceCreated();

            var enumerator = ((IEnumerable) TokenSequence).GetEnumerator();
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.As<ExportScriptToken>().Type.Should().Be(ExportScriptTokenType.Word);
            enumerator.Current.As<ExportScriptToken>().Content.Should().Be("lorem");
            enumerator.MoveNext().Should().BeFalse();
        }

        // --------------
        // Token checking
        // --------------

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyCheckWord()
        {
            GivenToken(ExportScriptTokenType.Word, "lorem");
            
            WhenTokenSequenceCreated();

            Then().HasWord().Should().BeTrue();
            Then().HasWord("lorem").Should().BeTrue();
            Then().HasWord("ipsum").Should().BeFalse();
            Then().HasSymbol().Should().BeFalse();
            Then().HasString().Should().BeFalse();

            Then().Next();
            Then().HasWord("lorem").Should().BeFalse(); // the cursor is no longer on "lorem" word
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyCheckSymbol()
        {
            GivenToken(ExportScriptTokenType.Symbol, "!");
            
            WhenTokenSequenceCreated();

            Then().HasSymbol().Should().BeTrue();
            Then().HasSymbol("!").Should().BeTrue();
            Then().HasSymbol("?").Should().BeFalse();
            Then().HasWord().Should().BeFalse();
            Then().HasString().Should().BeFalse();
            
            Then().Next();
            Then().HasSymbol("!").Should().BeFalse(); // the cursor is no longer on "!" symbol
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyCheckString()
        {
            GivenToken(ExportScriptTokenType.String, "Some text");
            
            WhenTokenSequenceCreated();

            Then().HasString().Should().BeTrue();
            Then().HasString("Some text").Should().BeTrue();
            Then().HasString("Other text").Should().BeFalse();
            Then().HasWord().Should().BeFalse();
            Then().HasSymbol().Should().BeFalse();
            
            Then().Next();
            Then().HasString("Some text").Should().BeFalse(); // the cursor is no longer on "Some text" symbol
        }
        
        // ---------------
        // Token accepting
        // ---------------
        
        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyAcceptWord()
        {
            GivenToken(ExportScriptTokenType.Word, "lorem");
            
            WhenTokenSequenceCreated();

            Then().AcceptWord("ipsum").Should().BeNull();
            Then().AcceptSymbol().Should().BeNull();
            Then().AcceptString().Should().BeNull();

            Then().AcceptWord("lorem").Should().NotBeNull();
            Then().AcceptWord("lorem").Should().BeNull(); // the cursor is no longer on "lorem" word
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyAcceptSymbol()
        {
            GivenToken(ExportScriptTokenType.Symbol, "!");
            
            WhenTokenSequenceCreated();

            Then().AcceptSymbol("?").Should().BeNull();
            Then().AcceptWord().Should().BeNull();
            Then().AcceptString().Should().BeNull();
            
            Then().AcceptSymbol("!").Should().NotBeNull();
            Then().AcceptSymbol("!").Should().BeNull(); // the cursor is no longer on "!" symbol
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyAcceptString()
        {
            GivenToken(ExportScriptTokenType.String, "Some text");
            
            WhenTokenSequenceCreated();

            Then().AcceptString("Other text").Should().BeNull();
            Then().AcceptWord().Should().BeNull();
            Then().AcceptSymbol().Should().BeNull();
            
            Then().AcceptString("Some text").Should().NotBeNull();
            Then().AcceptString("Some text").Should().BeNull(); // the cursor is no longer on "Some text" symbol
        }

        // --------------
        // Token skipping
        // --------------
        
        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlySkipWord()
        {
            GivenToken(ExportScriptTokenType.Word, "lorem");
            
            WhenTokenSequenceCreated();

            Then().SkipWord("ipsum").Should().BeFalse();
            Then().SkipSymbol().Should().BeFalse();
            Then().SkipString().Should().BeFalse();

            Then().SkipWord("lorem").Should().BeTrue();
            Then().SkipWord("lorem").Should().BeFalse(); // the cursor is no longer on "lorem" word
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlySkipSymbol()
        {
            GivenToken(ExportScriptTokenType.Symbol, "!");
            
            WhenTokenSequenceCreated();

            Then().SkipSymbol("?").Should().BeFalse();
            Then().SkipWord().Should().BeFalse();
            Then().SkipString().Should().BeFalse();
            
            Then().SkipSymbol("!").Should().BeTrue();
            Then().SkipSymbol("!").Should().BeFalse(); // the cursor is no longer on "!" symbol
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlySkipString()
        {
            GivenToken(ExportScriptTokenType.String, "Some text");
            
            WhenTokenSequenceCreated();

            Then().SkipString("Other text").Should().BeFalse();
            Then().SkipWord().Should().BeFalse();
            Then().SkipSymbol().Should().BeFalse();
            
            Then().SkipString("Some text").Should().BeTrue();
            Then().SkipString("Some text").Should().BeFalse(); // the cursor is no longer on "Some text" symbol
        }

        // ---------------
        // Token expecting
        // ---------------
        
        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyExpectWord()
        {
            GivenToken(ExportScriptTokenType.Word, "lorem");
            
            WhenTokenSequenceCreated();

            ThenAttempt(() => TokenSequence.ExpectWord("ipsum")).Should().ThrowExactly<UnexpectedTokenException>();
            ThenAttempt(() => TokenSequence.ExpectSymbol()).Should().ThrowExactly<UnexpectedTokenException>();
            ThenAttempt(() => TokenSequence.ExpectString()).Should().ThrowExactly<UnexpectedTokenException>();

            Then().ExpectWord("lorem").Should().BeOfType<ExportScriptToken>();
            ThenAttempt(() => TokenSequence.ExpectWord("lorem")).Should().ThrowExactly<UnexpectedTokenException>()
                .Which.ActualToken.Should().BeNull();
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyExpectSymbol()
        {
            GivenToken(ExportScriptTokenType.Symbol, "!");
            
            WhenTokenSequenceCreated();

            ThenAttempt(() => TokenSequence.ExpectSymbol("?")).Should().ThrowExactly<UnexpectedTokenException>();
            ThenAttempt(() => TokenSequence.ExpectWord()).Should().ThrowExactly<UnexpectedTokenException>();
            ThenAttempt(() => TokenSequence.ExpectString()).Should().ThrowExactly<UnexpectedTokenException>();

            Then().ExpectSymbol("!").Should().BeOfType<ExportScriptToken>();
            ThenAttempt(() => TokenSequence.ExpectSymbol("!")).Should().ThrowExactly<UnexpectedTokenException>()
                .Which.ActualToken.Should().BeNull();
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyExpectString()
        {
            GivenToken(ExportScriptTokenType.Word, "lorem");
            
            WhenTokenSequenceCreated();

            ThenAttempt(() => TokenSequence.ExpectEndOfTokens()).Should().ThrowExactly<UnexpectedTokenException>();
            Then().SkipWord();
            ThenAttempt(() => TokenSequence.ExpectEndOfTokens()).Should().NotThrow();
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldProperlyExpectEndOfTokens()
        {
            GivenNoTokens();
            GivenCursor(11, 2, 3);
            
            WhenTokenSequenceCreated();

            var exception = ThenAttempt(() => TokenSequence.ExpectWord())
                .Should().ThrowExactly<UnexpectedTokenException>().Which;
            exception.Start.Should().Be(new Cursor(11, 2, 3));
            exception.End.Should().Be(new Cursor(11, 2, 3));
        }

        [Fact]
        public void ExportScriptTokenSequence_ShouldPassEndPositionForUnexpectedEndOfTokens()
        {
            GivenNoTokens();
            GivenCursor(11, 2, 3);
            
            WhenTokenSequenceCreated();

            var exception = ThenAttempt(() => TokenSequence.ExpectWord())
                .Should().ThrowExactly<UnexpectedTokenException>().Which;
            exception.Start.Should().Be(new Cursor(11, 2, 3));
            exception.End.Should().Be(new Cursor(11, 2, 3));
        }

        // -----
        // Setup
        // -----
        
        private Cursor Cursor { get; set; }
        private ICollection<ExportScriptToken> Tokens { get; set; } = new List<ExportScriptToken>();

        private ExportScriptTokenSequence TokenSequence { get; set; } = default!;

        // Givens
        
        private void GivenNoTokens() { }
        private void GivenToken(ExportScriptTokenType tokenType, string content)
        {
            var start = Cursor;
            var end = Cursor = Cursor.AdvanceColumns(content.Length);
            var token = new ExportScriptToken(tokenType, content, start, end);
            Tokens.Add(token);
        }

        private void GivenCursor(int position, int line, int column)
            => Cursor = new Cursor(position, line, column);
        
        // Whens

        private void WhenTokenSequenceCreated()
        {
            TokenSequence = new ExportScriptTokenSequence(Tokens, Cursor);
        }
        
        // Thens

        private ExportScriptTokenSequence Then()
            => TokenSequence;
        private ExportScriptTokenSequence ThenSequence()
            => TokenSequence;

        private void ThenExpectCurrentToken(ExportScriptTokenType type, string content)
        {
            var current = TokenSequence.Current!;
            current.Type.Should().Be(type);
            current.Content.Should().Be(content);
        }
        
        private void ThenExpectNextToken(ExportScriptTokenType type, string content)
        {
            var current = TokenSequence.Next()!;
            current.Type.Should().Be(type);
            current.Content.Should().Be(content);
        }

        private void ThenExpectNoFurtherTokens()
        {
            TokenSequence.IsEndOfTokens.Should().BeTrue();
            TokenSequence.Current.Should().BeNull();
            TokenSequence.Next().Should().BeNull();
        }

        private Action ThenAttempt(Action attempt)
        {
            return attempt;
        }
    }
}