using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Alignments
{
    public class JamAlignmentTallyCalculator
    {
        public JamAlignmentTally CalculateResults(JamVoteCollection votesCollection)
        {
            var alignmentVotes = votesCollection.Votes
                .Where(vote => vote.Ranking.Any())
                .Select(CalculateSingleVote)
                .ToList();

            var baseScores = CalculateBaseScores(alignmentVotes);
            var reviewScores = CalculateReviewScores(votesCollection);
            var themeBonuses = CalculateThemeBonuses(votesCollection);
            var totalScores = CalculateTotalScores(baseScores, reviewScores, themeBonuses);

            var ranking = totalScores.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();

            return new JamAlignmentTally
            {
                Votes = alignmentVotes,
                BaseScores = baseScores,
                ReviewScores = reviewScores,
                ThemeBonuses = themeBonuses,
                TotalScores = totalScores,
                AlignmentRanking = ranking,
            };
        }

        private JamAlignmentVote CalculateSingleVote(JamVote vote)
        {
            var rankedEntries = vote.Ranking
                .Where(entry => entry.Alignment != null && entry.Alignment != vote.Alignment)
                .ToList();
            var unrankedEntries = vote.Missing
                .Where(entry => entry.Alignment != null && entry.Alignment != vote.Alignment)
                .ToList();
            var votedEntries = rankedEntries.Concat(unrankedEntries).ToList();

            var totalCount = votedEntries.Count;
            var rankedCounts = rankedEntries
                .GroupBy(entry => entry.Alignment!)
                .ToDictionary(group => group.Key, group => group.Count());
            var unrankedCounts = unrankedEntries
                .GroupBy(entry => entry.Alignment!)
                .ToDictionary(group => group.Key, group => group.Count());
            var alignedCounts = votedEntries
                .GroupBy(entry => entry.Alignment!)
                .ToDictionary(group => group.Key, group => group.Count());
            var competingCounts = alignedCounts.ToDictionary(kvp => kvp.Key, kvp => totalCount - kvp.Value);

            // calcualting individual entries
            var ongoingTotal = 0;
            var ongoingCounts = alignedCounts.ToDictionary(kvp => kvp.Key, kvp => 0);
            var entryScores = new List<JamAlignmentEntryScore>();
            foreach (var entry in rankedEntries)
            {
                ongoingTotal++;
                ongoingCounts[entry.Alignment!]++;

                var competingAbove = ongoingTotal - ongoingCounts[entry.Alignment!];
                var competingTotal = competingCounts[entry.Alignment!];
                var entryScore = new JamAlignmentEntryScore()
                {
                    Entry = entry,
                    IsRanked = true,
                    CompetingEntriesAbove = competingAbove,
                    CompetingEntriesTotal = competingTotal,
                };
                entryScores.Add(entryScore);
            }
            foreach (var entry in unrankedEntries)
            {
                var competingAbove = ongoingTotal - ongoingCounts[entry.Alignment!];
                var competingTotal = competingCounts[entry.Alignment!];
                var entryScore = new JamAlignmentEntryScore()
                {
                    Entry = entry,
                    IsRanked = false,
                    CompetingEntriesAbove = competingAbove,
                    CompetingEntriesTotal = competingTotal,
                };
                entryScores.Add(entryScore);
            }

            // calculating averages
            var rankedAverages = entryScores.Where(score => score.IsRanked)
                .GroupBy(score => score.Entry.Alignment!)
                .ToDictionary(group => group.Key, group => group.Average(score => score.AlignmentScore));
            var unrankedAverages = entryScores.Where(score => !score.IsRanked)
                .GroupBy(score => score.Entry.Alignment!)
                .ToDictionary(group => group.Key, group => group.Average(score => score.AlignmentScore));
            var alignedAverages = entryScores
                .GroupBy(score => score.Entry.Alignment!)
                .ToDictionary(group => group.Key, group => group.Average(score => score.AlignmentScore));

            // returning the result
            return new JamAlignmentVote
            {
                OriginalVote = vote,

                TotalRankedCount = rankedEntries.Count,
                TotalUnrankedCount = unrankedEntries.Count,
                TotalEntriesCount = votedEntries.Count,

                RankedEntriesCounts = rankedCounts,
                RankedAverages = rankedAverages,
                UnrankedEntriesCounts = unrankedCounts,
                UnrankedAverages = unrankedAverages,
                AlignedEntriesCounts = alignedCounts,
                AlignedAverages = alignedAverages,

                EntryScores = entryScores,
            };
        }

        private IReadOnlyDictionary<JamAlignmentOption, decimal> CalculateBaseScores(IReadOnlyCollection<JamAlignmentVote> votes)
        {
            var options = JamTallyModel.Current.Jam!.Alignments!.GetAvailableOptions();
            var voteAverages = votes.SelectMany(vote => vote.AlignedAverages).ToList();
            return voteAverages
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(group => group.Key, group => group.Average(kvp => kvp.Value));
        }

        private IReadOnlyDictionary<JamAlignmentOption, JamAlignmentReviewScore> CalculateReviewScores(JamVoteCollection votesCollection)
        {
            var jam = JamTallyModel.Current.Jam!;
            var votelessEntries = new HashSet<JamEntry>(jam.Entries);

            var eligibleVotes = votesCollection.Votes.Where(vote => vote.Ranking.Count >= 20 && vote.ReviewsCount >= 20).ToList();
            foreach (var authoredEntry in eligibleVotes.SelectMany(vote => vote.Authored))
                votelessEntries.Remove(authoredEntry);

            foreach (var duplicate in votesCollection.AlignmentBattleData!.Duplicates)
                votelessEntries.Remove(duplicate);

            var result = new Dictionary<JamAlignmentOption, JamAlignmentReviewScore>();
            foreach (var alignment in jam.Alignments!.GetAvailableOptions())
            {
                var eligibleVotesCount = eligibleVotes.Count(vote => vote.Alignment == alignment);
                var votelessEntriesCount = votelessEntries.Count(entry => entry.Alignment == alignment);
                var reviewScore = new JamAlignmentReviewScore
                {
                    Alignment = alignment,
                    EligibleVotes = eligibleVotesCount,
                    VotelessEntries = votelessEntriesCount
                };
                result.Add(alignment, reviewScore);
            }
            return result;
        }

        private IReadOnlyDictionary<JamAlignmentOption, decimal> CalculateThemeBonuses(JamVoteCollection votesCollection)
        {
            var jam = JamTallyModel.Current.Jam!;
            var themeGuessed = votesCollection.AlignmentBattleData!.ThemeGuessed;
            return jam.Alignments!.GetAvailableOptions()
                .ToDictionary(alignment => alignment, alignment => alignment == themeGuessed ? 0.02m : 0.00m);
        }

        private IReadOnlyDictionary<JamAlignmentOption, decimal> CalculateTotalScores(
            IReadOnlyDictionary<JamAlignmentOption, decimal> baseScores,
            IReadOnlyDictionary<JamAlignmentOption, JamAlignmentReviewScore> reviewScores,
            IReadOnlyDictionary<JamAlignmentOption, decimal> themeBonuses
            )
        {
            var jam = JamTallyModel.Current.Jam!;
            var result = new Dictionary<JamAlignmentOption, decimal>();
            foreach (var alignment in jam.Alignments!.GetAvailableOptions())
            {
                var baseScore = baseScores[alignment];
                var reviewMultiplier = reviewScores[alignment].Multiplier;
                var themeBonus = themeBonuses[alignment];
                var totalScore = baseScore * reviewMultiplier + themeBonus;
                result.Add(alignment, totalScore);
            }
            return result;
        }
    }
}
