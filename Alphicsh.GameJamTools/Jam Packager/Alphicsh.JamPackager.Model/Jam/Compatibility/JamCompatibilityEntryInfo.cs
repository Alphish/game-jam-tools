using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamTools.Common.IO.Execution;

namespace Alphicsh.JamPackager.Model.Jam.Compatibility
{
    internal class JamCompatibilityEntryInfo
    {
        [JsonPropertyName("Title")]
        public string Title { get; init; } = default!;

        [JsonPropertyName("Authors")]
        public string Authors { get; init; } = default!;

        [JsonPropertyName("DirectoryName")]
        public string DirectoryName { get; init; } = default!;

        // Files

        [JsonPropertyName("ExecutablePath")]
        public string ExecutablePath { get; init; } = default!;

        [JsonPropertyName("PostJamPath")]
        public string? PostJamPath { get; } = null;

        [JsonPropertyName("ReadmePath")]
        public string? ReadmePath { get; init; } = default!;

        [JsonPropertyName("IsEmergencyReadme")]
        public bool IsEmergencyReadme { get; init; } = false;

        [JsonPropertyName("AfterwordPath")]
        public string? AfterwordPath { get; init; } = default!;

        // Ratings

        [JsonPropertyName("Overall")]
        public int Overall { get; } = 1;

        [JsonPropertyName("UpperTheme")]
        public int UpperTheme { get; } = 1;

        [JsonPropertyName("LowerTheme")]
        public int LowerTheme { get; } = 1;

        [JsonPropertyName("Concept")]
        public int Concept { get; } = 1;

        [JsonPropertyName("Presentation")]
        public int Presentation { get; } = 1;

        [JsonPropertyName("Story")]
        public int Story { get; } = 1;

        [JsonPropertyName("Comment")]
        public string Comment { get; init; } = string.Empty;

        // Conversion

        public static JamCompatibilityEntryInfo FromEntry(JamEntryEditable entry)
        {
            var executablePath = PrepareExecutable(entry);
            return new JamCompatibilityEntryInfo
            {
                Title = entry.DisplayShortTitle,
                Authors = entry.Team.DisplayName,
                DirectoryName = entry.Files.DirectoryPath.GetLastSegmentName(),
                ExecutablePath = executablePath,
                ReadmePath = entry.Files.Readme?.Location,
                IsEmergencyReadme = entry.Files.Readme?.IsRequired ?? false,
                AfterwordPath = entry.Files.Afterword?.Location,
            };
        }

        private static string PrepareExecutable(JamEntryEditable entry)
        {
            var launchers = entry.Files.Launchers;
            
            var executable = launchers.FirstOrDefault(launcher => launcher.Type == LaunchType.WindowsExe);
            if (executable != null)
            {
                return executable.Location;
            }

            var gxgame = launchers.FirstOrDefault(launcher => launcher.Type == LaunchType.GxGamesLink);
            if (gxgame != null)
            {
                var playBatPath = entry.Files.DirectoryPath.Append("play.bat").Value;
                if (!File.Exists(playBatPath))
                    File.WriteAllText(playBatPath, $"\"%LOCALAPPDATA%\\Programs\\Opera GX\\opera.exe\" \"{gxgame.Location}\"");

                return "play.bat";
            }

            throw new NotSupportedException($"Cannot prepare executable for '{entry.DisplayShortTitle} by {entry.Team.DisplayName}'.");
        }
    }
}
