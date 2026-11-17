using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Calculation
{
    public class JamTallyCalculator
    {
        private JamTallyRankScoreCalculator MainScoreCalculator { get; } = new JamTallyRankScoreCalculator(entryCounter => entryCounter.Ranks.Sum(rank => 1m / (rank + 1)));
        private JamTallyAlignmentCalculator AlignmentCalculator { get; } = new JamTallyAlignmentCalculator();
        private JamTallyAwardsCalculator AwardsCalculator { get; } = new JamTallyAwardsCalculator();

        public JamTallyResult Calculate(JamOverview jam, IReadOnlyList<JamVote> votes)
        {
            var tallyVotes = votes.Select(JamTallyVote.ForMainTally).ToList();

            var ranking = MainScoreCalculator.TallyRanking(jam.Entries, tallyVotes);
            var alignmentResult = AlignmentCalculator.Calculate(jam, votes);

            var awardsSummary = AwardsCalculator.CalculateAwards(jam.AwardCriteria, ranking);
            var topReviewScore = tallyVotes.Max(vote => vote.ReviewScore);
            var topReviewers = tallyVotes.Where(vote => vote.ReviewScore == topReviewScore).Select(vote => vote.Voter).ToList();

            return new JamTallyResult
            {
                Ranking = ranking,
                AlignmentResult = alignmentResult,
                AwardCriteria = awardsSummary.AwardCriteria,
                AwardEntryScores = awardsSummary.AwardEntryScores,
                AwardTopScores = awardsSummary.AwardTopScores,
                AwardWinners = awardsSummary.AwardWinners,
                Votes = tallyVotes,
                TopReviewScore = topReviewScore,
                TopReviewers = topReviewers,
            };
        }
    }
}
