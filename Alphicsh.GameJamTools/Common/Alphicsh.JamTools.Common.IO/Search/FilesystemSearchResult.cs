using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTools.Common.IO.Search
{
    public class FilesystemSearchResult
    {
        public IReadOnlyCollection<FilePath> SearchedPaths { get; }
        public IReadOnlyCollection<FilePath> FoundPaths { get; }

        public FilesystemSearchResult(IEnumerable<FilePath> searchedPaths, IEnumerable<FilePath> foundPaths)
        {
            SearchedPaths = searchedPaths.ToList();
            FoundPaths = foundPaths.ToList();
        }

        public FilesystemSearchResult ElseFindMatches(string pattern)
        {
            if (FoundPaths.Any())
                return this;

            return FilesystemPathsMatcher.MatchSearchResult(pattern, this.SearchedPaths);
        }

        public FilesystemSearchResult ElseFindAll()
        {
            if (FoundPaths.Any())
                return this;

            return new FilesystemSearchResult(this.SearchedPaths, foundPaths: this.SearchedPaths);
        }
    }
}
