using Alphicsh.JamTally.Model.Result;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class EntrySection
    {
        public TrophiesImage Image { get; init; } = default!;
        public JamTallyEntry Entry { get; init; } = default!;

        public int Row { get; init; }
        public int Column { get; init; }

        public int X { get; init; }
        public int Y { get; init; }

        public GameTrophyLayout Layout { get; init; } = default!;
        public TrophiesComposite MedalComposite { get; init; } = default!;
        public TrophiesComposite TextComposite { get; init; } = default!;

        public static EntrySection Create(TrophiesImage image, JamTallyEntry tallyEntry, int row, int column)
        {
            var x = column * image.ColumnWidth;
            var y = row * image.RowHeight;
            var layout = new GameTrophyLayout(x, y, image.TrophyWidth, image.TrophyHeight);
            var code = tallyEntry.Code;

            var medalComposite = image.DefineComposite($"entry_{code}", layout)
                .WithGuide(layer: "medals_inner", role: "medal_inner", layout.MedalArea, "302010")
                .WithGuide(layer: "medals_outer", role: "medal_outer", layout.MedalArea, "302010");

            var textComposite = image.DefineComposite($"entrytext_{code}", layout)
                .WithGuide(layer: "text_stroke", role: "authors_stroke", layout.AuthorsArea, "000000")
                .WithGuide(layer: "text_fill", role: "authors_fill", layout.AuthorsArea, "000000")
                .WithGuide(layer: "text_stroke", role: "title_stroke", layout.TitleArea, "102030")
                .WithGuide(layer: "text_fill", role: "title_fill", layout.TitleArea, "102030")
                .WithGuide(layer: "text_stroke", role: "desc_stroke", layout.DescriptionArea, "000000")
                .WithGuide(layer: "text_fill", role: "desc_fill", layout.DescriptionArea, "000000");

            return new EntrySection
            {
                Image = image,
                Entry = tallyEntry,
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
