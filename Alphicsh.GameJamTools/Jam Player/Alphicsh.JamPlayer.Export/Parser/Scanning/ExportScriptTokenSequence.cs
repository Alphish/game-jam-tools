using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamPlayer.Export.Parser.Parsing.Exceptions;

namespace Alphicsh.JamPlayer.Export.Parser.Scanning
{
    public class ExportScriptTokenSequence : IReadOnlyCollection<ExportScriptToken>
    {
        // ------
        // Basics
        // ------
        
        private IList<ExportScriptToken> Tokens { get; }
        private Cursor EndOfTextCursor { get; }

        public ExportScriptTokenSequence(IEnumerable<ExportScriptToken> tokens, Cursor endOfTextCursor)
        {
            Tokens = tokens.ToList();
            EndOfTextCursor = endOfTextCursor;
            
            Index = 0;
        }
        
        public int Count => Tokens.Count();
        public IEnumerator<ExportScriptToken> GetEnumerator()
            => Tokens.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
        
        private int Index { get; set; }
        public bool IsEndOfTokens => Index >= Count;
        public ExportScriptToken? Current => Index < Count ? Tokens[Index] : null;

        public ExportScriptToken? Next()
        {
            return Index < Count ? Tokens[Index++] : null;
        }

        // ---------------
        // Checking tokens
        // ---------------

        public bool HasTokenType(ExportScriptTokenType type)
            => Current?.Type == type;

        public bool HasContent(string content)
            => Current?.Content == content;

        public bool HasToken(ExportScriptTokenType type, string? content)
            => HasTokenType(type) && (content == null || HasContent(content));

        public bool HasWord(string? content = null)
            => HasToken(ExportScriptTokenType.Word, content);

        public bool HasSymbol(string? content = null)
            => HasToken(ExportScriptTokenType.Symbol, content);
        
        public bool HasString(string? content = null)
            => HasToken(ExportScriptTokenType.String, content);
        
        // ----------------
        // Accepting tokens
        // ----------------

        public ExportScriptToken? AcceptToken(ExportScriptTokenType type, string? content)
        {
            return HasToken(type, content) ? Next() : null;
        }

        public ExportScriptToken? AcceptWord(string? content = null)
            => AcceptToken(ExportScriptTokenType.Word, content);

        public ExportScriptToken? AcceptSymbol(string? content = null)
            => AcceptToken(ExportScriptTokenType.Symbol, content);

        public ExportScriptToken? AcceptString(string? content = null)
            => AcceptToken(ExportScriptTokenType.String, content);
        
        // ----------------
        // Skipping tokens
        // ----------------

        public bool SkipToken(ExportScriptTokenType type, string? content)
        {
            return AcceptToken(type, content) != null;
        }

        public bool SkipWord(string? content = null)
            => SkipToken(ExportScriptTokenType.Word, content);

        public bool SkipSymbol(string? content = null)
            => SkipToken(ExportScriptTokenType.Symbol, content);

        public bool SkipString(string? content = null)
            => SkipToken(ExportScriptTokenType.String, content);
        
        // ----------------
        // Expecting tokens
        // ----------------

        public void ExpectEndOfTokens()
        {
            if (!IsEndOfTokens)
                throw UnexpectedTokenError(expectedType: null, expectedContent: null);
        }
        
        public ExportScriptToken ExpectToken(ExportScriptTokenType type, string? content)
        {
            return HasToken(type, content) ? Next()! : throw UnexpectedTokenError(type, content);
        }

        public ExportScriptToken ExpectWord(string? content = null)
            => ExpectToken(ExportScriptTokenType.Word, content);

        public ExportScriptToken ExpectSymbol(string? content = null)
            => ExpectToken(ExportScriptTokenType.Symbol, content);

        public ExportScriptToken ExpectString(string? content = null)
            => ExpectToken(ExportScriptTokenType.String, content);

        private UnexpectedTokenException UnexpectedTokenError(
            ExportScriptTokenType? expectedType,
            string? expectedContent
        )
        {
            return new UnexpectedTokenException(
                expectedType, expectedContent, Current,
                Current?.Start ?? EndOfTextCursor, Current?.End ?? EndOfTextCursor
                );
        }
    }
}