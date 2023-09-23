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
            var totalScores = CalculateTotalScores(alignmentVotes);
            return new JamAlignmentTally
            {
                Votes = alignmentVotes,
                TotalScores = totalScores,
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

        private IReadOnlyDictionary<JamAlignmentOption, decimal> CalculateTotalScores(IReadOnlyCollection<JamAlignmentVote> votes)
        {
            var options = JamTallyModel.Current.Jam!.Alignments!.GetAvailableOptions();
            var voteAverages = votes.SelectMany(vote => vote.AlignedAverages).ToList();
            return voteAverages
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(group => group.Key, group => group.Average(kvp => kvp.Value));
        }
    }
}
