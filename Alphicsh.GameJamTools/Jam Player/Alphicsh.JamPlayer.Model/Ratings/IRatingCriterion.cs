namespace Alphicsh.JamPlayer.Model.Ratings
{
    public interface IRatingCriterion
    {
        string Id { get; }
        string Name { get; }

        IRating CreateRating();
    }

    public interface IRatingCriterion<TValue, TCriterion> : IRatingCriterion
        where TCriterion : IRatingCriterion<TValue, TCriterion>
    {
        new IRating<TValue, TCriterion> CreateRating();
    }
}
