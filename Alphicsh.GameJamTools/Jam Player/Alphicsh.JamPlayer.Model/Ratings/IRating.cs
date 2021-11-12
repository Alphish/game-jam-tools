namespace Alphicsh.JamPlayer.Model.Ratings
{
    public interface IRating
    {
        string Id { get; }
        string Name { get; }
        bool HasValue { get; }
    }
    
    public interface IRating<TValue> : IRating
    {
        TValue Value { get; set; }
    }

    public interface IRating<TValue, TOptions> : IRating<TValue>
        where TOptions : IRatingOptions<TValue>
    {
        TOptions Options { get; }
    }
}
