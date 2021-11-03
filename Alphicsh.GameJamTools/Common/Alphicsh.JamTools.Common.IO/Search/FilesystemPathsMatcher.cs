using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Alphicsh.JamTools.Common.IO.Search
{
    internal class FilesystemPathsMatcher
    {
        public static FilesystemSearchResult MatchSearchResult(string pattern, IEnumerable<FilePath> searchedPaths)
        {
            var foundPaths = FindPathsMatching(pattern, searchedPaths).ToList();
            return new FilesystemSearchResult(searchedPaths, foundPaths);
        }

        private static IEnumerable<FilePath> FindPathsMatching(string pattern, IEnumerable<FilePath> searchedPaths)
        {
            var asteriskPattern = Regex.Escape("*");
            var questionmarkPattern = Regex.Escape("?");

            var regexPattern = "^" + Regex.Escape(pattern) + "$";
            regexPattern = regexPattern.Replace(asteriskPattern, ".*");
            regexPattern = regexPattern.Replace(questionmarkPattern, ".");

            var regex = new Regex(regexPattern);
            foreach (var path in searchedPaths)
            {
                var lastSegmentName = path.GetLastSegmentName();
                if (regex.IsMatch(lastSegmentName))
                    yield return path;
            }
        }
    }
}
