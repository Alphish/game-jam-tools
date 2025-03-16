using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Result.Alignments;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyCalculator
    {
        private static JamAlignmentTallyCalculator AlignmentTallyCalculator { get; } = new JamAlignmentTallyCalculator();

        public JamTallyResult CalculateResults(JamVoteCollection votesCollection)
        {
            var jam = JamTallyModel.Current.Jam!;
            var talliedVotes = votesCollection.Votes.Where(vote => vote.Ranking.Any()).ToList();
            var finalRanking = CalculateFinalRanking(jam, talliedVotes);
            var awardRankings = CalculateAwardRankings(jam, talliedVotes);

            var alignmentTally = JamTallyModel.Current.Jam?.Alignments != null
                ? AlignmentTallyCalculator.CalculateResults(votesCollection)
                : null;

            return new JamTallyResult
            {
                Awards = jam.AwardCriteria,
                Entries = jam.Entries,
                Votes = talliedVotes,

                EntriesCount = jam.Entries.Count,
                AwardsCount = jam.AwardCriteria.Count,
                UnjudgedMaxCount = talliedVotes.Max(vote => vote.Unjudged.Count),
                ReactionsMaxCount = talliedVotes.Max(vote => vote.Reactions.Count),

                FinalRanking = finalRanking,
                AwardRankings = awardRankings,

                AlignmentTally = alignmentTally,
            };
        }

        // -------------------
        // Calculating results
        // -------------------

        internal IReadOnlyCollection<JamTallyEntryScore> CalculateFinalRanking(JamOverview jam, IReadOnlyCollection<JamVote> votes)
        {
            var entryScores = jam.Entries.ToDictionary(
                entry => entry,
                entry => new JamTallyEntryScore { Entry = entry, TotalVotesCount = votes.Count }
                );

            foreach (var vote in votes)
            {
                var rank = 1;
                foreach (var entry in vote.Ranking)
                {
                    entryScores[entry].AddRank(rank);
                    rank++;
                }

                var totalUnjudged = vote.Unjudged.Union(vote.Authored);
                foreach (var entry in totalUnjudged)
                {
                    entryScores[entry].MarkUnjudged();
                }
            }

            return entryScores.Values
                .OrderByDescending(entryScore => entryScore.TotalScore)
                .ToList();
        }

        private IReadOnlyCollection<JamTallyAwardRanking> CalculateAwardRankings(JamOverview jam, IReadOnlyCollection<JamVote> votes)
        {
            var nominations = votes.SelectMany(vote => vote.Awards);
            var awardScores = nominations
                .GroupBy(nomination => nomination)
                .Select(group => new JamTallyAwardScore { VoteAward = group.Key, Count = group.Count() })
                .ToList();

            var awardRankings = awardScores
                .GroupBy(awardScore => awardScore.Award)
                .Select(group => new JamTallyAwardRanking
                {
                    Award = group.Key,
                    Scores = group.OrderByDescending(score => score.Count).ThenBy(score => score.Entry.Line).ToList()
                }).ToDictionary(ranking => ranking.Award);

            return jam.AwardCriteria.Select(criterion => awardRankings[criterion]).ToList();
        }
    }
}
