using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteReaction
    {
        public JamReactionType Type { get; init; } = default!;
        public string User { get; init; } = default!;

        public string Name => User;
        public int Value => Type.Value;

        public string Line => $"{Type.Name} {User}";
        public override string ToString() => Line;
    }
}
