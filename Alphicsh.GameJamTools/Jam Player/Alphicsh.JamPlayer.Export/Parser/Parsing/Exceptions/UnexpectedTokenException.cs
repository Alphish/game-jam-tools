using Alphicsh.JamPlayer.Export.Parser.Scanning;

namespace Alphicsh.JamPlayer.Export.Parser.Parsing.Exceptions
{
    public class UnexpectedTokenException : ExportScriptParsingException
    {
        public ExportScriptTokenType? ExpectedType { get; }
        public string? ExpectedContent { get; }
        public ExportScriptToken? ActualToken { get; }
        
        public UnexpectedTokenException(
            ExportScriptTokenType? expectedType,
            string? expectedContent,
            ExportScriptToken? actualToken,
            Cursor start,
            Cursor end
            ) : base(BuildMessageForTokens(expectedType, expectedContent, actualToken), start, end)
        {
            ExpectedType = expectedType;
            ExpectedContent = expectedContent;
            ActualToken = actualToken;
        }

        private static string BuildMessageForTokens(
            ExportScriptTokenType? expectedType,
            string? expectedContent,
            ExportScriptToken? actualToken
            )
        {
            var actualTokenPart = actualToken != null
                ? $"Unexpected {actualToken.Type} token with '{actualToken.Content}' content."
                : $"Unexpected end of tokens.";

            string expectedTokenPart;
            if (expectedType == null)
                expectedTokenPart = "End of tokens expected.";
            else if (expectedContent == null)
                expectedTokenPart = $"{expectedType} token expected.";
            else
                expectedTokenPart = $"{expectedType} token with '{expectedContent}' content expected.";

            return $"{actualTokenPart} {expectedTokenPart}";
        }
    }
}