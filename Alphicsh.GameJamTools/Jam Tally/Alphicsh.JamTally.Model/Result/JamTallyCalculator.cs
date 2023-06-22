using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result
{
    public class JamTallyCalculator
    {
        public JamTallyResult CalculateResults(JamVoteCollection votesCollection)
        {
            var jam = JamTallyModel.Current.Jam!;
            var finalRanking = CalculateFinalRanking(jam, votesCollection);
            var awardRankings = CalculateAwardRankings(votesCollection);

            return new JamTallyResult
            {
                Awards = jam.AwardCriteria,
                Entries = jam.Entries,
                Votes = votesCollection.Votes.ToList(),
                FinalRanking = finalRanking,
                AwardRankings = awardRankings,
            };
        }

        // -------------------
        // Calculating results
        // -------------------

        private IReadOnlyCollection<JamTallyEntryScore> CalculateFinalRanking(JamOverview jam, JamVoteCollection votesCollection)
        {
            var entryScores = jam.Entries.ToDictionary(
                entry => entry,
                entry => new JamTallyEntryScore { Entry = entry, TotalVotesCount = votesCollection.Votes.Count }
                );

            foreach (var vote in votesCollection.Votes)
            {
                var rank = 1;
                foreach (var entry in vote.Ranking)
                {
                    entryScores[entry].AddRank(rank);
                    rank++;
                }

                foreach (var entry in vote.Unjudged)
                {
                    entryScores[entry].MarkUnjudged();
                }
            }

            return entryScores.Values
                .OrderByDescending(entryScore => entryScore.TotalScore)
                .ToList();
        }

        private IReadOnlyDictionary<JamAwardCriterion, JamTallyAwardRanking> CalculateAwardRankings(JamVoteCollection votesCollection)
        {
            var nominations = votesCollection.Votes.SelectMany(vote => vote.Awards);
            var awardScores = nominations
                .GroupBy(nomination => nomination)
                .Select(group => new JamTallyAwardScore { VoteAward = group.Key, Count = group.Count() })
                .ToList();

            return awardScores
                .GroupBy(awardScore => awardScore.Award)
                .Select(group => new JamTallyAwardRanking
                {
                    Award = group.Key,
                    Scores = group.OrderByDescending(score => score.Count).ThenBy(score => score.Entry.Line).ToList()
                }).ToDictionary(ranking => ranking.Award);
        }
    }
}
