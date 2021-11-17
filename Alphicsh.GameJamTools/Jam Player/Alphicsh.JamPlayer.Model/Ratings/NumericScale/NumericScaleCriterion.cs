namespace Alphicsh.JamPlayer.Model.Ratings.NumericScale
{
    public class NumericScaleCriterion : IRatingCriterion<double?, NumericScaleCriterion>
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;

        public double ValueStep { get; set; }
        public double MaxValue { get; set; }
        public INumericScaleSkin Skin { get; set; } = default!;

        IRating IRatingCriterion.CreateRating() => this.CreateRating();
        public IRating<double?, NumericScaleCriterion> CreateRating()
        {
            return new NumericScaleRating { Criterion = this, Value = null };
        }
    }
}
