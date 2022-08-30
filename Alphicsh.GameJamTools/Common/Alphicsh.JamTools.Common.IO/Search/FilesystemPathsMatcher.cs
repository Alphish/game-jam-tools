using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Alphicsh.JamTools.Common.IO.Search
{
    internal class FilesystemPathsMatcher
    {
        public static IReadOnlyCollection<FilePath> MatchSearchResult(string pattern, IEnumerable<FilePath> searchedPaths)
        {
            return MatchSearchResult(includes: pattern, excludes: null, searchedPaths);
        }

        public static IReadOnlyCollection<FilePath> MatchSearchResult(string includes, string? excludes, IEnumerable<FilePath> searchedPaths)
        {
            var splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
            var includesList = includes.Split(';', splitOptions);
            var excludesList = excludes?.Split(';', splitOptions) ?? Enumerable.Empty<string>();
            return FindPathsMatching(includesList, excludesList, searchedPaths).ToList();
        }

        private static IEnumerable<FilePath> FindPathsMatching(
            IEnumerable<string> includes,
            IEnumerable<string> excludes,
            IEnumerable<FilePath> searchedPaths
            )
        {
            var includeRegexes = includes.Select(CreateRegexFromPattern).ToList();
            var excludeRegexes = excludes.Select(CreateRegexFromPattern).ToList();

            foreach (var path in searchedPaths)
            {
                var lastSegmentName = path.GetLastSegmentName();

                if (excludeRegexes.Any(regex => regex.IsMatch(lastSegmentName)))
                    continue;

                if (includeRegexes.Any(regex => regex.IsMatch(lastSegmentName)))
                    yield return path;
            }
        }

        private static Regex CreateRegexFromPattern(string pattern)
        {
            var asteriskPattern = Regex.Escape("*");
            var questionmarkPattern = Regex.Escape("?");

            var regexPattern = "^" + Regex.Escape(pattern) + "$";
            regexPattern = regexPattern.Replace(asteriskPattern, ".*");
            regexPattern = regexPattern.Replace(questionmarkPattern, ".");

            return new Regex(regexPattern, RegexOptions.IgnoreCase);
        }
    }
}
