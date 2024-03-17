using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote.Search;

namespace Alphicsh.JamTally.Model.Vote.Serialization.Parsing
{
    internal class JamVotesFileParser
    {
        private JamOverview Jam { get; }
        private JamSearch JamSearch { get; }

        public JamVotesFileParser(JamOverview jam, JamSearch jamSearch)
        {
            Jam = jam;
            JamSearch = jamSearch;
        }

        public IList<JamVote> ParseVotes(string content)
        {
            var lines = content
                .Replace('\r', '\n')
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            var votesContents = SplitIntoVotesContents(lines);
            var votes = votesContents.Select(ParseContent).ToList();
            return votes;
        }

        // ----------------------
        // Reading votes contents
        // ----------------------

        private IReadOnlyCollection<JamVoteContent> SplitIntoVotesContents(IEnumerable<string> lines)
        {
            var result = new List<JamVoteContent>();

            var remainingLines = new Queue<string>(lines);
            while (remainingLines.Count > 0)
            {
                var voteContent = ReadVoteContent(remainingLines);
                result.Add(voteContent);
            }

            return result;
        }

        private JamVoteContent ReadVoteContent(Queue<string> remainingLines)
        {
            var header = remainingLines.Dequeue();
            if (!JamVoteContent.IsValidHeader(header))
                throw new InvalidOperationException($"Invalid vote header '{header}'.");

            var sections = new List<JamVoteSection>();
            while (remainingLines.Count > 0)
            {
                var currentLine = remainingLines.Peek();
                if (JamVoteContent.IsValidHeader(currentLine))
                    break;

                var newSection = ReadVoteSection(remainingLines);
                sections.Add(newSection);
            }

            return JamVoteContent.CreateForHeader(header, sections);
        }

        private JamVoteSection ReadVoteSection(Queue<string> remainingLines)
        {
            var header = remainingLines.Dequeue();
            if (!JamVoteSection.IsValidHeader(header))
                throw new InvalidOperationException($"Invalid vote section title '{header}'.");

            var sectionLines = new List<string>();
            while (remainingLines.Count > 0)
            {
                var currentLine = remainingLines.Peek();
                if (JamVoteContent.IsValidHeader(currentLine) || JamVoteSection.IsValidHeader(currentLine))
                    break;

                currentLine = remainingLines.Dequeue();
                if (currentLine.StartsWith("\\"))
                    currentLine = currentLine.Substring(1);

                sectionLines.Add(currentLine);
            }

            return JamVoteSection.CreateForHeader(header, sectionLines);
        }

        // -------------
        // Parsing votes
        // -------------

        private JamVote ParseContent(JamVoteContent content)
        {
            var voteParser = CreateParserForContent(content);
            return voteParser.Parse();
        }

        private JamVoteParserBase CreateParserForContent(JamVoteContent content)
        {
            switch (content.Version.ToLowerInvariant())
            {
                case "":
                    return new JamVoteParserLegacy(content, Jam, JamSearch);

                default:
                    throw new NotSupportedException($"The vote version '{content.Version}' is not supported.");
            }
        }
    }
}
