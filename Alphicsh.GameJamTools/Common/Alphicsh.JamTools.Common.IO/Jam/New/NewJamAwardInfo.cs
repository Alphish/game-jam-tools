using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam.New
{
    public class NewJamAwardInfo
    {
        public string Id { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string? Description { get; init; } = default!;

        [JsonIgnore] public string FixedName => Name ?? Description!;
        [JsonIgnore] public string? FixedDescription => Name != null ? Description : null;
    }
}
