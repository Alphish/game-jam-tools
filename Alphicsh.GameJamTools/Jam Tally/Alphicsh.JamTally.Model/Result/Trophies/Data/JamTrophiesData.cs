using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Trophies.Data
{
    public class JamTrophiesData
    {
        public TrophiesInput Input { get; init; } = default!;
        public int EntriesCount => Input.Entries.Count;

        public IReadOnlyCollection<JamTrophy> Trophies { get; init; } = default!;

        public static JamTrophiesData Compile(JamTallyResult tallyResult)
        {
            var input = TrophiesInput.Parse();

            var trophyEntriesByJamEntry = input.Entries.ToDictionary(entry => entry.Entry);
            var awardsByJamEntry = tallyResult.AwardRankings
                .SelectMany(ranking => ranking.GetWinners())
                .ToLookup(score => score.Entry, score => score.Award);

            var trophies = new List<JamTrophy>();
            for (var i = 0; i < tallyResult.EntriesCount; i++)
            {
                var rank = i + 1;
                var jamEntry = tallyResult.FinalRanking.ElementAt(i).Entry;
                var trophyEntry = trophyEntriesByJamEntry[jamEntry];
                var jamAwards = awardsByJamEntry[jamEntry].ToList();

                trophies.AddRange(GenerateTrophiesForEntry(rank, trophyEntry, jamAwards));
            }
            return new JamTrophiesData { Input = input, Trophies = trophies };
        }

        private static IEnumerable<JamTrophy> GenerateTrophiesForEntry(int rank, JamTrophyEntry entry, IReadOnlyList<JamAwardCriterion> jamAwards)
        {
            var result = new List<JamTrophy>();

            result.Add(JamTrophy.ForRank(entry, rank));
            if (jamAwards.Any())
            {
                var awardTrophies = jamAwards.Select((award, idx) => JamTrophy.ForAward(entry, award, rank, idx));
                result.AddRange(awardTrophies);
                result.Add(JamTrophy.ForAll(entry, rank, jamAwards));
            }

            return result;
        }
    }
}
