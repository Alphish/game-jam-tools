namespace Alphicsh.JamPlayer.Model.Ratings.NumericScale
{
    public sealed class NumericScaleRating : IRating<double?, NumericScaleOptions>
    {
        public string Id { get; init; } = default!;
        public string Name { get; init; } = default!;
        public NumericScaleOptions Options { get; init; } = default!;

        public double? Value { get; set; }
        public bool HasValue => Value.HasValue;
    }
}
