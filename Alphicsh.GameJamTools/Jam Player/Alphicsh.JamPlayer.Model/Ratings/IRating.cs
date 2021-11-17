namespace Alphicsh.JamPlayer.Model.Ratings
{
    public interface IRating
    {
        string Id { get; }
        string Name { get; }
        object? Value { get; set; }
        bool HasValue { get; }
    }
    
    public interface IRating<TValue> : IRating
    {
        new TValue? Value { get; set; }
    }

    public interface IRating<TValue, TCriterion> : IRating<TValue>
        where TCriterion : IRatingCriterion<TValue, TCriterion>
    {
        TCriterion Criterion { get; }
    }
}
