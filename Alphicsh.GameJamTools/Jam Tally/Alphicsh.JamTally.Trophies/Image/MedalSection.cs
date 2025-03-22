namespace Alphicsh.JamTally.Trophies.Image
{
    public class MedalSection
    {
        public TrophiesImage Image { get; init; } = default!;
        public string MedalType { get; init; } = default!;

        public int Row { get; init; }
        public int Column { get; init; }

        public int X { get; init; }
        public int Y { get; init; }

        public GameTrophyLayout Layout { get; init; } = default!;
        public TrophiesComposite MedalComposite { get; init; } = default!;
        public TrophiesComposite TextComposite { get; init; } = default!;

        public static MedalSection Create(TrophiesImage image, string medalType, int row, int column)
        {
            var x = column * image.ColumnWidth;
            var y = row * image.RowHeight;
            var layout = new GameTrophyLayout(x, y, image.TrophyWidth, image.TrophyHeight);

            var medalComposite = image.CreateComposite($"medal_{medalType}", layout)
                .WithGuide(layer: "medals_back", role: "medal_back", layout.MedalArea, "302010")
                .WithGuide(layer: "medals_rim", role: "medal_rim", layout.MedalArea, "302010")
                .WithGuide(layer: "medals_sheen", role: "medal_sheen", layout.MedalArea, "302010");

            var textComposite = image.CreateComposite($"medaltext_{medalType}", layout)
                .WithGuide(layer: "text_fill", role: "jam_label", layout.JamLabelArea, "000000")
                .WithGuide(layer: "text_stroke", role: "authors_stroke", layout.AuthorsArea, "000000")
                .WithGuide(layer: "text_fill", role: "authors_fill", layout.AuthorsArea, "000000")
                .WithGuide(layer: "text_stroke", role: "title_stroke", layout.TitleArea, "102030")
                .WithGuide(layer: "text_fill", role: "title_fill", layout.TitleArea, "102030")
                .WithGuide(layer: "text_stroke", role: "desc_stroke", layout.DescriptionArea, "000000")
                .WithGuide(layer: "text_fill", role: "desc_fill", layout.DescriptionArea, "000000");

            return new MedalSection
            {
                Image = image,
                Row = row,
                Column = column,
                X = x,
                Y = y,
                Layout = layout,
                MedalComposite = medalComposite,
                TextComposite = textComposite,
            };
        }
    }
}
