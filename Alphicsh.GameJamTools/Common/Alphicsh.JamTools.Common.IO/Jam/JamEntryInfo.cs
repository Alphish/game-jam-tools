using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamEntryInfo
    {
        [JsonIgnore] public string Id { get; internal set; } = default!;

        // ----------------
        // Basic properties
        // ----------------

        public string Title { get; init; } = default!;
        public JamTeamInfo Team { get; init; } = default!;

        // ---------------------
        // Filesystem properties
        // ---------------------

        [JsonIgnore] public FilePath EntryInfoPath { get; set; }
        [JsonIgnore] public FilePath EntryDirectoryPath
        {
            get => EntryInfoPath.GetParentDirectoryPath()!.Value;
            set => EntryInfoPath = value.Append(EntryInfoFileName);
        }
        [JsonIgnore] public string EntryInfoFileName
        {
            get => EntryInfoPath.GetLastSegmentName();
            set => EntryInfoPath = EntryDirectoryPath.Append(value);
        }
    }
}
