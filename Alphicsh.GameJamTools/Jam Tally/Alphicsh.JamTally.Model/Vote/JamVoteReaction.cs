namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteReaction
    {
        public string Name { get; init; }
        public int Value { get; init; }

        public string Line => $"+{Value} {Name}";
        public override string ToString() => Line;
    }
}
