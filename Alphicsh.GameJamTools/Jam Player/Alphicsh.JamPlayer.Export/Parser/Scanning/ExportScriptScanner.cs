using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alphicsh.JamPlayer.Export.Parser.Scanning.Exceptions;

namespace Alphicsh.JamPlayer.Export.Parser.Scanning
{
    using KnownSymbolsLookup = IReadOnlyCollection<KeyValuePair<int, HashSet<string>>>;

    public class ExportScriptScanner
    {
        public ExportScriptTokenSequence ScanContent(string content)
        {
            var operation = new ScanningOperation(content);
            return operation.ScanTokenSequence();
        }

        private class ScanningOperation
        {
            // ----------
            // Basic data
            // ----------
            
            private static bool[] WordCharset { get; } = Enumerable.Range(0, 128)
                .Select(i => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_".Contains((char)i))
                .ToArray();

            private static KnownSymbolsLookup KnownSymbols { get; } = CreateKnownSymbolsLookup(new string[]
            {
                ".", "+", "-", "*", "/", "%"
            });

            private static KnownSymbolsLookup CreateKnownSymbolsLookup(IEnumerable<string> symbols)
            {
                return symbols.GroupBy(symbol => symbol.Length)
                    .ToDictionary(group => group.Key, group => group.ToHashSet())
                    .OrderByDescending(kvp => kvp.Key)
                    .ToList();
            }

            private static IReadOnlyDictionary<string, string> EscapeSequences { get; } =
                new Dictionary<string, string>
                {
                    ["\\t"] = "\t",
                    ["\\n"] = "\n",
                    ["\\r"] = "\r",
                    ["\\'"] = "'",
                    ["\\\""] = "\"",
                    ["\\\\"] = "\\",
                };
            
            // -----
            // Setup
            // -----
            
            private string Content { get; }
            private string ParsedContent { get; }
            private int Length => Content.Length;
            
            private Cursor Cursor { get; set; }
            private int Position => Cursor.Position;
            private char? Current => Position < Length ? ParsedContent[Position] : null;
            
            public ScanningOperation(string content)
            {
                Content = content;
                ParsedContent = NormalizeParsedContent(content);
                Cursor = new Cursor(0, 0, 0);
            }

            private string NormalizeParsedContent(string content)
            {
                return content
                    .Replace("\t", " ")
                    .Replace("\r\n", " \n")
                    .Replace("\r", "\n");
            }
            
            // ----------------
            // General scanning
            // ----------------

            public ExportScriptTokenSequence ScanTokenSequence()
            {
                var tokens = ScanAllTokens();
                return new ExportScriptTokenSequence(tokens, Cursor);
            }

            private IEnumerable<ExportScriptToken> ScanAllTokens()
            {
                while (Current.HasValue)
                {
                    switch (Current)
                    {
                        case ' ':
                            SkipSpace();
                            break;
                        
                        case '\n':
                            SkipNewlines();
                            break;
                        
                        case '/':
                            if (HasAhead("//"))
                                SkipLineComment();
                            else if (HasAhead("/*"))
                                SkipBlockComment();
                            else
                                yield return ScanSymbolOrFail();
                            break;
                        
                        case '\'':
                        case '"':
                            yield return ScanString();
                            break;
                        
                        default:
                            if (WordCharset.Length < Current.Value)
                                throw new UnexpectedCharacterException(Current.Value, Cursor);
                            if (WordCharset[Current.Value])
                                yield return ScanWord();
                            else
                                yield return ScanSymbolOrFail();
                            break;
                    }
                }
            }
            
            // ----------------------------
            // Skipping spaces and comments
            // ----------------------------

            private void SkipSpace()
            {
                var pos = Position;
                while (pos < Length && ParsedContent[pos] == ' ')
                {
                    pos++;
                }

                Cursor = Cursor.AdvanceColumns(pos - Position);
            }

            private void SkipNewlines()
            {
                while (Current == '\n')
                {
                    Cursor = Cursor.AdvanceLine();
                }
            }

            private void SkipLineComment()
            {
                var pos = ParsedContent.IndexOf('\n', Position);
                if (pos == -1)
                    pos = Length;

                Cursor = Cursor.AdvanceColumns(pos - Position);
                Cursor.AdvanceLine();
            }

            private void SkipBlockComment()
            {
                var pos = ParsedContent.IndexOf("*/", Position, StringComparison.Ordinal);
                if (pos == -1)
                    throw new UnmatchedBlockCommentException(Cursor);

                pos += "*/".Length;
                var commentContent = ParsedContent.Substring(Position, pos - Position);
                var newlineCount = commentContent.Count(c => c == '\n');
                var lastNewlineIndex = ParsedContent.LastIndexOf('\n', pos - 1);
                if (lastNewlineIndex == -1)
                    lastNewlineIndex = 0;
                
                Cursor = new Cursor(pos, Cursor.Line + newlineCount, pos - lastNewlineIndex);
            }
            
            // ---------------
            // Actual scanning
            // ---------------

            private ExportScriptToken ScanWord()
            {
                var pos = Position;
                while (pos < Length && WordCharset[ParsedContent[pos]])
                {
                    pos++;
                }

                var startCursor = Cursor;
                var endCursor = Cursor = Cursor.AdvanceColumns(pos - Position);
                var tokenContent = ParsedContent.Substring(startCursor.Position, Position - startCursor.Position);

                return new ExportScriptToken(ExportScriptTokenType.Word, tokenContent, startCursor, endCursor);
            }

            private ExportScriptToken ScanSymbolOrFail()
            {
                foreach (var symbolGroup in KnownSymbols)
                {
                    var symbolLength = symbolGroup.Key;
                    if (Position + symbolLength > Length)
                        continue;

                    var potentialSymbol = ParsedContent.Substring(Position, symbolLength);
                    if (symbolGroup.Value.Contains(potentialSymbol))
                    {
                        var startCursor = Cursor;
                        var endCursor = Cursor = Cursor.AdvanceColumns(symbolLength);
                        return new ExportScriptToken(ExportScriptTokenType.Symbol, potentialSymbol, startCursor, endCursor);
                    }
                }

                throw new UnexpectedCharacterException(ParsedContent[Position], Cursor);
            }

            private ExportScriptToken ScanString()
            {
                var startCursor = Cursor;
                var delimiter = Current;
                var advancePos = Position + 1; // omitting the delimiter character
                var collectedPos = advancePos;

                var contentBuilder = new StringBuilder();
                while (advancePos < Length)
                {
                    var advanceChar = ParsedContent[advancePos];
                    switch (advanceChar)
                    {
                        case '\n':
                            throw new UnmatchedStringException(Cursor);
                        case '\\':
                            var advanceString = Content.Substring(collectedPos, advancePos - collectedPos);
                            contentBuilder.Append(advanceString);
                            
                            var sequence = GetEscapeSequenceAhead(advancePos);
                            contentBuilder.Append(EscapeSequences[sequence]);
                            advancePos += sequence.Length;
                            collectedPos = advancePos;
                            break;
                        default:
                            if (advanceChar == delimiter)
                            {
                                var restString = Content.Substring(collectedPos, advancePos - collectedPos);
                                contentBuilder.Append(restString);
                                var content = contentBuilder.ToString();

                                advancePos += 1; // delimiter length
                                var endCursor = Cursor = Cursor.AdvanceColumns(advancePos - Position);
                                return new ExportScriptToken(ExportScriptTokenType.String, content, startCursor, endCursor);
                            }
                            else
                            {
                                advancePos++;
                            }
                            break;
                    }
                }
                throw new UnmatchedStringException(Cursor);
            }

            private string GetEscapeSequenceAhead(int advancePos)
            {
                var sequence = ParsedContent.Substring(advancePos, length: 2);
                if (!EscapeSequences.ContainsKey(sequence))
                    throw new UnknownEscapeSequenceException(sequence, Cursor.AdvanceColumns(advancePos - Position));

                return sequence;
            }
            
            // ---------
            // Utilities
            // ---------

            private bool HasAhead(string str)
            {
                var startIndex = Position;
                var endIndex = Position + str.Length;
                if (endIndex > Length)
                    return false;

                return ParsedContent.Substring(startIndex, str.Length) == str;
            }
        }
    }
}