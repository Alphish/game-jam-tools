using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Alphicsh.JamPackager.Model.Jam.Compatibility
{
    internal class JamCompatibilityInfo
    {
        // General data

        [JsonPropertyName("GamesDirectory")]
        public string GamesDirectory { get; init; } = default!;

        [JsonPropertyName("Title")]
        public string Title { get; init; } = default!;

        [JsonPropertyName("LowerTheme")]
        public string LowerTheme { get; } = "Theme";

        [JsonPropertyName("UpperTheme")]
        public string? UpperTheme { get; } = null;

        // Entries

        [JsonPropertyName("Remaining")]
        public IReadOnlyCollection<JamCompatibilityEntryInfo> Remaining { get; init; } = default!;

        [JsonPropertyName("Ranking")]
        public IReadOnlyCollection<JamCompatibilityEntryInfo> Ranking { get; } = new List<JamCompatibilityEntryInfo>();

        [JsonPropertyName("Unranked")]
        public IReadOnlyCollection<JamCompatibilityEntryInfo> Unranked { get; } = new List<JamCompatibilityEntryInfo>();

        // Best ofs

        [JsonPropertyName("BestUpperThemeName")]
        public string BestUpperThemeName { get; } = string.Empty;

        [JsonPropertyName("BestLowerThemeName")]
        public string BestLowerThemeName { get; } = string.Empty;

        [JsonPropertyName("BestConceptName")]
        public string BestConceptName { get; } = string.Empty;

        [JsonPropertyName("BestPresentationName")]
        public string BestPresentationName { get; } = string.Empty;

        [JsonPropertyName("BestStoryName")]
        public string BestStoryName { get; } = string.Empty;
        
        [JsonPropertyName("BestDevlogName")]
        public string BestDevlogName { get; } = string.Empty;

        [JsonPropertyName("IsAutosaving")]
        public bool IsAutosaving { get; } = false;

        // Conversion

        public static JamCompatibilityInfo FromJam(JamEditable jam)
        {
            return new JamCompatibilityInfo
            {
                GamesDirectory = jam.EntriesLocation,
                Title = jam.Title ?? string.Empty,
                Remaining = jam.Entries.Select(entry => JamCompatibilityEntryInfo.FromEntry(entry)).ToList(),
            };
        }
    }
}
