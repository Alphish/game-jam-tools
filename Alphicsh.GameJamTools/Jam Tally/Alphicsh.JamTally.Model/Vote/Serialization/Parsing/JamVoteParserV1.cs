using System;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote.Search;

namespace Alphicsh.JamTally.Model.Vote.Serialization.Parsing
{
    internal class JamVoteParserV1 : JamVoteParserBase
    {
        public JamVoteParserV1(JamVoteContent content, JamOverview jam, JamSearch jamSearch)
            : base(content, jam, jamSearch)
        {
        }

        public override string Version => "V1";

        public override JamVote Parse()
        {
            var sectionsLookup = Content.Sections.ToLookup(section => section.Title, StringComparer.OrdinalIgnoreCase);
            var duplicateGroup = sectionsLookup.FirstOrDefault(group => group.Count() > 1);
            if (duplicateGroup != null)
                throw new InvalidOperationException($"Duplicate section '{duplicateGroup.First().Header}' found.");

            var sections = sectionsLookup.ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

            var vote = new JamVote();

            if (sections.ContainsKey("VOTER"))
                ProcessVoterSection(vote, sections["VOTER"]);

            // Entries

            if (sections.ContainsKey("AUTHORED"))
                vote.Authored = ParseEntriesSection(sections["AUTHORED"], unordered: true);

            if (sections.ContainsKey("RANKING"))
                vote.Ranking = ParseEntriesSection(sections["RANKING"], unordered: false);

            if (sections.ContainsKey("UNJUDGED"))
            {
                vote.Unjudged = ParseEntriesSection(sections["UNJUDGED"], unordered: false)
                    .Except(vote.Authored)
                    .ToList();
            }

            // Awards

            if (sections.ContainsKey("AWARDS"))
                vote.Awards = ParseAwardsSection(sections["AWARDS"]);

            // Reviews

            if (sections.ContainsKey("REVIEWED"))
                ProcessReviewedSection(vote, sections["REVIEWED"]);

            // Reactions

            if (sections.ContainsKey("REACTIONS"))
                vote.Reactions = ParseReactionsSection(sections["REACTIONS"]);

            CompleteVote(vote);
            return vote;
        }

        private void ProcessReviewedSection(JamVote vote, JamVoteSection section)
        {
            var countLines = section.Lines.Where(line => line.StartsWith("=")).ToList();
            if (countLines.Count > 1)
                throw new InvalidOperationException($"Multiple review counts were given: {string.Join(",", countLines)}");

            if (countLines.Count == 1)
            {
                var countString = countLines[0].Substring(1).Trim();
                if (!int.TryParse(countString, out var reviewsCount))
                    throw new InvalidOperationException($"Could not read review count from '{countLines[0]}'");

                vote.DirectReviewsCount = reviewsCount;
            }

            var entryLines = section.Lines.Where(line => !line.StartsWith("=")).ToList();
            vote.ReviewedEntries = entryLines.Select(ParseEntryLine)
                .OrderBy(entry => entry.FullLine)
                .ToList();
        }
    }
}
