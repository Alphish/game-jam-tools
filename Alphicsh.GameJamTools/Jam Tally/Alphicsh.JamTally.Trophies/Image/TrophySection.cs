using System.Linq;
using Alphicsh.JamTally.Model.Result;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class TrophySection
    {
        public TrophiesImage Image { get; init; } = default!;
        public JamTallyEntry Entry { get; init; } = default!;
        public string TrophyType { get; init; } = default!;
        public string MedalType { get; init; } = default!;
        public string Description { get; init; } = default!;

        public string ExportId => $"{Entry.Code}_{TrophyType}_export";

        public int Row { get; init; }
        public int Column { get; init; }

        public int X { get; init; }
        public int Y { get; init; }

        public static TrophySection Create(TrophiesImage image, JamTallyEntry entry, string trophyType, int row, int column)
        {
            var x = column * image.ColumnWidth;
            var y = row * image.RowHeight;

            return new TrophySection
            {
                Image = image,
                Entry = entry,
                TrophyType = trophyType,
                MedalType = ChooseMedalType(entry, trophyType),
                Description = ResolveDescription(entry, trophyType),
                Row = row,
                Column = column,
                X = x,
                Y = y,
            };
        }

        private static string ChooseMedalType(JamTallyEntry entry, string trophyType)
        {
            if (trophyType != "rank" && trophyType != "all")
                return "award";

            if (entry.Rank == 1)
                return "gold";
            else if (entry.Rank == 2)
                return "silver";
            else if (entry.Rank == 3)
                return "bronze";
            else
                return trophyType == "all" ? "award" : "basic";
        }

        private static string ResolveDescription(JamTallyEntry entry, string trophyType)
        {
            if (trophyType == "rank")
                return GetRankDescription(entry.Rank);
            else if (trophyType == "all")
                return GetCompleteDescription(entry);
            else
                return entry.Awards.First(award => award.Id.Equals(trophyType)).Name;
        }

        private static string GetRankDescription(int rank)
        {
            var mod100 = rank % 100;
            if (mod100 >= 10 && mod100 <= 20)
                return $"{rank}th place";

            var mod10 = rank % 10;
            switch (mod10)
            {
                case 1:
                    return $"{rank}st place";
                case 2:
                    return $"{rank}nd place";
                case 3:
                    return $"{rank}rd place";
                default:
                    return $"{rank}th place";
            }
        }

        private static string GetCompleteDescription(JamTallyEntry entry)
        {
            var awardNames = entry.Awards.Select(award => award.Name).ToList();
            awardNames.Insert(0, GetRankDescription(entry.Rank));
            return string.Join(", ", awardNames);
        }
    }
}
