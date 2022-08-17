using System.Collections.Generic;
using System.Text.Json.Serialization;

using Alphicsh.JamTools.Common.IO.Jam.Files;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamInfo
    {
        // -------
        // Entries
        // -------

        public FilePath EntriesSubpath { get; init; } = default!;

        [JsonPropertyName("entries")] public IReadOnlyCollection<JamEntryStub> EntriesStubs { get; init; } = default!;
        [JsonIgnore] public IReadOnlyCollection<JamEntryInfo> Entries { get; internal set; } = default!;

        public IReadOnlyCollection<JamAwardInfo> AwardCriteria { get; init; } = default!;

        // ---------------------
        // Filesystem properties
        // ---------------------

        [JsonIgnore] public FilePath JamInfoPath { get; set; }
        [JsonIgnore]
        public FilePath JamDirectoryPath
        {
            get => JamInfoPath.GetParentDirectoryPath()!.Value;
            set => JamInfoPath = value.Append(JamInfoFileName);
        }
        [JsonIgnore]
        public string JamInfoFileName
        {
            get => JamInfoPath.GetLastSegmentName();
            set => JamInfoPath = JamDirectoryPath.Append(value);
        }

        // -------
        // Loading
        // -------

        public static JamInfo? LoadFromFile(FilePath jamInfoPath)
        {
            var explorer = new JamInfoExplorer();
            return explorer.TryLoadJamInfo(jamInfoPath);
        }

        public static JamInfo LoadFromDirectory(FilePath jamDirectoryPath)
        {
            var explorer = new JamInfoExplorer();
            return explorer.FindJamInfo(jamDirectoryPath);
        }

        public static JamInfo RediscoverFromDirectory(FilePath jamDirectoryPath)
        {
            var explorer = new JamInfoExplorer();
            return explorer.RediscoverJamInfo(jamDirectoryPath);
        }

        // ------
        // Saving
        // ------

        public void Save()
        {
            var writer = new JamFilesWriter();
            writer.SaveJamInfo(this);
        }
    }
}
