using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamInfo
    {
        [JsonIgnore] public FilePath JamDirectoryPath { get; internal set; } = default!;
        [JsonIgnore] public FilePath? JamInfoPath { get; internal set; }

        public FilePath EntriesSubpath { get; init; } = default!;

        [JsonPropertyName("entries")] public IReadOnlyCollection<JamEntryStub> EntriesStubs { get; init; } = default!;
        [JsonIgnore] public IReadOnlyCollection<JamEntryInfo> Entries { get; internal set; } = default!;
    }
}
