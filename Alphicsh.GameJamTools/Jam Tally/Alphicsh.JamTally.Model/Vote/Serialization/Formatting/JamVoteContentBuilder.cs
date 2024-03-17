using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote.Serialization.Formatting
{
    internal class JamVoteContentBuilder
    {
        private const string VoteVersion = "V1";

        private JamOverview Jam { get; }

        public JamVoteContentBuilder(JamOverview jam)
        {
            Jam = jam;
        }

        public JamVoteContent BuildVoteContent(JamVote vote)
        {
            var sections = CreateSectionsForVote(vote).ToList();
            return JamVoteContent.CreateForVersion(VoteVersion, sections);
        }

        private IEnumerable<JamVoteSection> CreateSectionsForVote(JamVote vote)
        {
            yield return CreateVoterSection("VOTER", vote.Voter!, vote.Alignment);

            if (vote.Authored.Any())
                yield return CreateEntriesSection("AUTHORED", vote.Authored);
            
            if (vote.Ranking.Any())
                yield return CreateEntriesSection("RANKING", vote.Ranking);
            
            if (vote.Unjudged.Any())
                yield return CreateEntriesSection("UNJUDGED", vote.Unjudged);

            if (vote.Awards.Any())
                yield return CreateAwardsSection("AWARDS", vote.Awards);
            
            if (vote.HasDirectReviewsCount || vote.ReviewedEntries.Any())
                yield return CreateReviewedSection("REVIEWED", vote.DirectReviewsCount, vote.ReviewedEntries);
            
            if (vote.Reactions.Any())
                yield return CreateReactionsSection("REACTIONS", vote.Reactions);
        }

        private JamVoteSection CreateVoterSection(string title, string voter, JamAlignmentOption? option)
        {
            var sectionLines = new List<string>();
            sectionLines.Add(voter);

            if (Jam.Alignments != null)
                sectionLines.Add(option?.Title ?? Jam.Alignments.NeitherTitle);

            return JamVoteSection.CreateForTitle(title, sectionLines);
        }

        private JamVoteSection CreateEntriesSection(string title, IEnumerable<JamEntry> entries)
        {
            var sectionLines = entries.Select(entry => entry.FullLine);
            return JamVoteSection.CreateForTitle(title, sectionLines);
        }

        private JamVoteSection CreateAwardsSection(string title, IEnumerable<JamVoteAward> awards)
        {
            var sectionLines = awards.Select(entry => entry.FullLine);
            return JamVoteSection.CreateForTitle(title, sectionLines);
        }

        private JamVoteSection CreateReviewedSection(string title, int? count, IEnumerable<JamEntry> entries)
        {
            var sectionLines = entries.Select(entry => entry.FullLine).ToList();
            if (count.HasValue && count > 0)
                sectionLines.Insert(0, $"={count}");

            return JamVoteSection.CreateForTitle(title, sectionLines);
        }

        private JamVoteSection CreateReactionsSection(string title, IEnumerable<JamVoteReaction> reactions)
        {
            var sectionLines = reactions.Select(reaction => reaction.Line);
            return JamVoteSection.CreateForTitle(title, sectionLines);
        }
    }
}
