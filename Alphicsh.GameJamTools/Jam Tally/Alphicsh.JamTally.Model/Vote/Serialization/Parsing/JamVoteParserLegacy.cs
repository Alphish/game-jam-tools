using System;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote.Search;

namespace Alphicsh.JamTally.Model.Vote.Serialization.Parsing
{
    internal class JamVoteParserLegacy : JamVoteParserBase
    {
        public JamVoteParserLegacy(JamVoteContent content, JamOverview jam, JamSearch jamSearch)
            : base(content, jam, jamSearch)
        {
        }

        public override string Version => string.Empty;

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

            if (sections.ContainsKey("STATS"))
                ProcessStatsSection(vote, sections["STATS"]);

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

            // Reactions

            if (sections.ContainsKey("REACTIONS"))
                vote.Reactions = ParseReactionsSection(sections["REACTIONS"]);

            CompleteVote(vote);
            return vote;
        }

        private void ProcessStatsSection(JamVote vote, JamVoteSection section)
        {
            foreach (var line in section.Lines)
            {
                if (line.StartsWith("Reviews count:", StringComparison.OrdinalIgnoreCase))
                {
                    var reviewsCountString = line.Substring("Reviews count:".Length).Trim();
                    if (!int.TryParse(reviewsCountString, out var reviewsCount))
                        throw new InvalidOperationException($"Could not parse reviews count from '{line}'.");

                    vote.DirectReviewsCount = reviewsCount;
                }
                else
                {
                    throw new InvalidOperationException($"Could not resolve stats line '{line}'.");
                }
            }
        }
    }
}
