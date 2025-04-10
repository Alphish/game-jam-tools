using Alphicsh.JamTally.Model.Result;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class ReviewerSection
    {
        public TrophiesImage Image { get; init; } = default!;
        public JamTallyVote Vote { get; init; } = default!;

        public string ExportId => $"reviewer_{Vote.VoterCode}_export";
        public string Voter => Vote.Voter;

        public int Row { get; init; }
        public int Column { get; init; }

        public int X { get; init; }
        public int Y { get; init; }

        public static ReviewerSection Create(TrophiesImage image, JamTallyVote vote, int row, int column)
        {
            var x = column * image.ColumnWidth;
            var y = row * image.RowHeight;

            return new ReviewerSection
            {
                Image = image,
                Vote = vote,
                Row = row,
                Column = column,
                X = x,
                Y = y,
            };
        }
    }
}
