using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamEntryInfo
    {
        [JsonIgnore] public string Id { get; internal set; } = default!;
        [JsonIgnore] public FilePath EntryDirectoryPath { get; internal set; } = default!;
        [JsonIgnore] public FilePath? EntryInfoPath { get; internal set; } = default!;

        public string Title { get; init; } = default!;
        public JamTeamInfo Team { get; init; } = default!;
    }
}
