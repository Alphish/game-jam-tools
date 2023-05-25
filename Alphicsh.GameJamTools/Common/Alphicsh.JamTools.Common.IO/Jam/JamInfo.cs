using System.Collections.Generic;
using System.Text.Json.Serialization;

using Alphicsh.JamTools.Common.IO.Jam.Serialization;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamInfo
    {
        // -------------------
        // General information
        // -------------------

        public string? Title { get; init; } = default!;
        public FilePath? LogoFileName { get; set; } = default!;
        public string? Theme { get; init; } = default!;

        // -------
        // Entries
        // -------

        public FilePath EntriesSubpath { get; init; } = default!;
        [JsonPropertyName("entries")] public IReadOnlyCollection<JamEntryStub> EntriesStubs { get; init; } = default!;
        [JsonIgnore] public IReadOnlyCollection<JamEntryLegacyInfo> Entries { get; internal set; } = default!;

        public IReadOnlyCollection<JamAwardInfo> AwardCriteria { get; init; } = default!;

        // ---------------------
        // Filesystem properties
        // ---------------------

        [JsonIgnore] public FilePath JamInfoPath { get; set; }

        [JsonIgnore] public FilePath JamDirectoryPath
        {
            get => JamInfoPath.GetParentDirectoryPath()!.Value;
            set => JamInfoPath = value.Append(JamInfoFileName);
        }

        [JsonIgnore] public string JamInfoFileName
        {
            get => JamInfoPath.GetLastSegmentName();
            set => JamInfoPath = JamDirectoryPath.Append(value);
        }

        [JsonIgnore] public FilePath? LogoPath
            => JamDirectoryPath.AppendNullable(LogoFileName);

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
