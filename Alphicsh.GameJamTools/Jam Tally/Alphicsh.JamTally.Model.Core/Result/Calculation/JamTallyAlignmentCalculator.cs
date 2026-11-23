using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    public class JamTallyAlignmentCalculator
    {
        private JamTallyRankScoreCalculator AlignmentScoreCalculator { get; } = new JamTallyRankScoreCalculator(entryCounter => entryCounter.Ranks.Sum(rank => 1m / (rank + 3)));

        public JamTallyAlignmentResult? Calculate(JamOverview jam, IReadOnlyList<JamVote> votes)
        {
            if (jam.Alignments == null)
                return null;

            var tallyVotes = votes.Select(JamTallyVote.ForAlignmentTally).ToList();
            var alignments = jam.Alignments!.GetAvailableOptions().ToList();

            var entries = jam.Entries.Where(entry => entry.Alignment != null).ToList();
            var entriesRanking = AlignmentScoreCalculator.TallyRanking(entries, tallyVotes);
            var entryScoresByAlignment = entriesRanking.ToLookup(entry => entry.Alignment);
            var baseScores = alignments.ToDictionary(alignment => alignment, alignment => entryScoresByAlignment[alignment].Sum(entry => entry.TotalScore));

            var fullVotesByAlignment = tallyVotes
                .Where(vote => vote.VoterAlignment != null && vote.IsComplete)
                .ToLookup(vote => vote.VoterAlignment);
            var reviewCounts = alignments.ToDictionary(alignment => alignment, alignment => fullVotesByAlignment[alignment].Count());
            var themeGuesses = alignments.ToDictionary(alignment => alignment, alignment => alignment.Id == "purple"); // temporary hardcoding, because making a whole infrastructure to mark which gang had theme guessed would be unnecessarily time-consuming
            var multipliers = alignments.ToDictionary(alignment => alignment, alignment => 1.00m + 0.05m * reviewCounts[alignment] + (themeGuesses[alignment] ? 0.01m : 0.00m));

            var totalScores = alignments.ToDictionary(alignment => alignment, alignment => baseScores[alignment] * multipliers[alignment]);

            var alignmentsRanking = alignments.Select(alignment => new JamTallyAlignment
            {
                Alignment = alignment,
                BaseScore = baseScores[alignment],
                ReviewsCount = reviewCounts[alignment],
                GuessedTheme = themeGuesses[alignment],
                ScoreMultiplier = multipliers[alignment],
                TotalScore = totalScores[alignment],
            }).OrderByDescending(tally => tally.TotalScore).ToList();

            return new JamTallyAlignmentResult
            {
                Entries = entriesRanking,
                Ranking = alignmentsRanking,
                Votes = tallyVotes,
            };
        }
    }
}
