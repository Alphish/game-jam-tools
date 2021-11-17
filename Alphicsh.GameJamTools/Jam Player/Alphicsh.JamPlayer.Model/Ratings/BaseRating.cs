namespace Alphicsh.JamPlayer.Model.Ratings
{
    public abstract class BaseRating<TValue, TCriterion> : IRating<TValue, TCriterion>
        where TCriterion : IRatingCriterion<TValue, TCriterion>
    {
        public TCriterion Criterion { get; init; } = default!;
        public string Id => Criterion.Id;
        public string Name => Criterion.Name;

        public TValue? Value { get; set; } = default!;
        public abstract bool HasValue { get; }

        object? IRating.Value { get => Value; set => Value = FromObjectValue(value); }
        protected abstract TValue? FromObjectValue(object? value);
    }
}
