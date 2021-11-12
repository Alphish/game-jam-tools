namespace Alphicsh.JamPlayer.Model.Ratings.NumericScale
{
    public class NumericScaleOptions : IRatingOptions<double?>
    {
        public double ValueStep { get; set; }
        public double MaxValue { get; set; }
        public INumericScaleSkin Skin { get; set; } = default!;
    }
}
