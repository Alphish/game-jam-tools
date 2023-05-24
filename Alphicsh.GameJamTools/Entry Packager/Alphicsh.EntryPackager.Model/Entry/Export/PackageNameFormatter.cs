using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Alphicsh.EntryPackager.Model.Entry.Export
{
    public class PackageNameFormatter
    {
        public string GetPackageName(string title, string team)
        {
            var sanitizedTitle = Sanitize(title);
            if (string.IsNullOrWhiteSpace(sanitizedTitle))
                sanitizedTitle = "Entry"; // handling cases when title has no ASCII characters whatsoever

            var sanitizedTeam = Sanitize(team);
            if (string.IsNullOrWhiteSpace(sanitizedTeam))
                sanitizedTeam = "Team"; // same, but for the team name

            return sanitizedTitle + " by " + sanitizedTeam;
        }

        private string Sanitize(string text)
        {
            text = text.Trim();
            text = Substitute(text);
            text = FilterInvalidCharacters(text);
            if (text.Length > 40)
                text = text.Substring(0, 40);
            
            return text;
        }

        // ------------
        // Latinization
        // ------------

        private static Dictionary<char, string> DirectMappings { get; } = new Dictionary<char, string>()
        {
            [':'] = " -",
            ['Ł'] = "L",
            ['ł'] = "l",
        };

        private string Substitute(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString.ToCharArray())
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory == UnicodeCategory.NonSpacingMark)
                    continue;

                if (DirectMappings.ContainsKey(c))
                    stringBuilder.Append(DirectMappings[c]);
                else
                    stringBuilder.Append(c);
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        // ---------
        // Filtering
        // ---------

        private string FilterInvalidCharacters(string text)
        {
            return Regex.Replace(text, "[^0-9A-Za-z '.,!()_-]", "");
        }
    }
}
