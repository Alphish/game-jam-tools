using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Trophies.Data
{
    public class JamTrophy
    {
        public JamTrophyEntry Entry { get; init; } = default!;
        public string Qualifier { get; init; } = default!;
        public string Medal { get; init; } = default!;
        public string Description { get; init; } = default!;
        public int XOffset { get; init; }
        public int YOffset { get; init; }

        public int X => XOffset * 600;
        public int Y => YOffset * 140;

        public static JamTrophy ForRank(JamTrophyEntry entry, int rank)
        {
            return new JamTrophy
            {
                Entry = entry,
                Qualifier = "rank",
                Medal = GetMedalForRank(rank) ?? "basic",
                Description = $"{rank}{GetOrderSuffix(rank)} place",
                XOffset = 0,
                YOffset = rank - 1,
            };
        }

        public static JamTrophy ForAward(JamTrophyEntry entry, JamAwardCriterion criterion, int rank, int awardIdx)
        {
            return new JamTrophy
            {
                Entry = entry,
                Qualifier = criterion.Id,
                Medal = "award",
                Description = criterion.Name,
                XOffset = awardIdx + 1,
                YOffset = rank - 1,
            };
        }

        public static JamTrophy ForAll(JamTrophyEntry entry, int rank, IReadOnlyCollection<JamAwardCriterion> criteria)
        {
            var descriptionSegments = new List<string> { $"{rank}{GetOrderSuffix(rank)} place" };
            foreach (var criterion in criteria)
                descriptionSegments.Add(criterion.Name);

            return new JamTrophy
            {
                Entry = entry,
                Qualifier = "all",
                Medal = GetMedalForRank(rank) ?? "award",
                Description = string.Join(", ", descriptionSegments),
                XOffset = -1,
                YOffset = rank - 1,
            };
        }

        private static string? GetMedalForRank(int rank)
        {
            switch (rank)
            {
                case 1:
                    return "gold";
                case 2:
                    return "silver";
                case 3:
                    return "bronze";
                default:
                    return null;
            }
        }

        private static string GetOrderSuffix(int rank)
        {
            var mod100 = rank % 100;
            if (mod100 >= 10 && mod100 < 20)
                return "th";

            var mod10 = rank % 10;
            switch (mod10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}
