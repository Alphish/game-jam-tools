namespace Alphicsh.JamTally.Trophies.Image.Generators
{
    public class TrophiesCoreGenerator
    {
        public TrophiesImage Generate(TrophiesCoreSettings coreSettings)
        {
            var image = TrophiesImage.CreateBlank(coreSettings.TrophyWidth, coreSettings.TrophyHeight, coreSettings.ColumnWidth, coreSettings.RowHeight)
                .WithLayer("medals_back", "Medals Back")
                .WithLayer("medals_rim", "Medals Rim")
                .WithLayer("medals_sheen", "Medals Sheen")
                .WithLayer("text_stroke", "Text Stroke")
                .WithLayer("text_fill", "Text Fill")
                .WithMedalSection("gold", row: -5, column: 0)
                .WithMedalSection("silver", row: -4, column: 0)
                .WithMedalSection("bronze", row: -3, column: 0)
                .WithMedalSection("award", row: -2, column: 0)
                .WithMedalSection("basic", row: -1, column: 0);

            return image;
        }
    }
}
