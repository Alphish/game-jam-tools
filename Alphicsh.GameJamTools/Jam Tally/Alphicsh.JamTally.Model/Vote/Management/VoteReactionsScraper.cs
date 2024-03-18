using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Alphicsh.JamTally.Model.Vote.Management
{
    public class VoteReactionsScraper
    {
        private static string BlockRowSubpattern { get; }
            = "<li[^>]*class=\"[^\"]*block-row[^\"]*\"[^>]*>";
        
        private static string ReactionTitleSubpattern { get; }
            = "<img[^>]*class=\"reaction-sprite[^\"]*\"[^>]*title=\"(\\w+)\">";

        private static string UsernameSubpattern { get; }
            = "<a[^>]*class=\"[^\"]*username[^\"]*\"[^>]*>(<span[^<]*>)?([^<]*)";

        private static Regex VoteReactionPattern { get; } = new Regex(
            $"{BlockRowSubpattern}.*?{ReactionTitleSubpattern}.*?{UsernameSubpattern}",
            RegexOptions.Singleline | RegexOptions.IgnoreCase
            );

        public string ScrapeReactions(string content)
        {
            var matches = VoteReactionPattern.Matches(content).AsEnumerable().ToList();
            if (matches.Count == 0)
                return "No matches found";

            var lines = new List<string>();
            foreach (var match in matches)
            {
                var reaction = match.Groups[1].Value;
                var user = match.Groups[3].Value;
                lines.Add($"{reaction} {user}");
            }
            return string.Join("\n", lines);
        }
    }
}
