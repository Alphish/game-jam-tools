using System;
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

            var foundPaths = FilesystemPathsMatcher.MatchSearchResult(pattern, this.SearchedPaths);
            return new FilesystemSearchResult(this.SearchedPaths, foundPaths);
        }

        public FilesystemSearchResult ElseFindMatches(string includes, string excludes)
        {
            if (FoundPaths.Any())
                return this;

            var foundPaths = FilesystemPathsMatcher.MatchSearchResult(includes, excludes, this.SearchedPaths);
            return new FilesystemSearchResult(this.SearchedPaths, foundPaths);
        }

        public FilesystemSearchResult ElseFindAll()
        {
            if (FoundPaths.Any())
                return this;

            return new FilesystemSearchResult(this.SearchedPaths, foundPaths: this.SearchedPaths);
        }

        public FilePath? FirstOrDefault()
        {
            return FoundPaths.Any() ? FoundPaths.First() : null;
        }
    }
}
