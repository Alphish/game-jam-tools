using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alphicsh.JamTools.Common.IO.Search
{
    public class FilesystemSearch
    {
        private FilePath SearchedDirectoryPath { get; }
        private bool IsDirectoriesSearch { get; }
        private EnumerationOptions EnumerationOptions { get; }
        private HashSet<string>? AllowedExtensions { get; set; }

        private FilesystemSearch(FilePath directory, bool isDirectoriesSearch)
        {
            SearchedDirectoryPath = directory;
            IsDirectoriesSearch = isDirectoriesSearch;

            EnumerationOptions = new EnumerationOptions
            {
                AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
                BufferSize = 0,
                IgnoreInaccessible = true,
                MatchCasing = MatchCasing.CaseInsensitive,
                MatchType = MatchType.Simple,
                ReturnSpecialDirectories = false,

                RecurseSubdirectories = false,
            };
        }

        // -------------
        // Configuration
        // -------------

        public static FilesystemSearch ForDirectoriesIn(FilePath directory)
        {
            return new FilesystemSearch(directory, isDirectoriesSearch: true);
        }

        public static FilesystemSearch ForFilesIn(FilePath directory)
        {
            return new FilesystemSearch(directory, isDirectoriesSearch: false);
        }

        public FilesystemSearch IncludingTopDirectoryOnly()
        {
            EnumerationOptions.RecurseSubdirectories = false;
            return this;
        }

        public FilesystemSearch IncludingNestedDirectories()
        {
            EnumerationOptions.RecurseSubdirectories = true;
            return this;
        }

        public FilesystemSearch WithExtensions(IEnumerable<string> extensions)
        {
            if (IsDirectoriesSearch)
                throw new InvalidOperationException("The list of allowed extensions can only be set for a files search query.");

            if (AllowedExtensions != null)
                throw new InvalidOperationException("The list of allowed extensions cannot be set twice for the same query.");

            // allowed extensions are normalised so that they start with a dot
            // no matter what the user enters
            AllowedExtensions = extensions
                .Select(extension => extension.StartsWith(".") ? extension : "." + extension)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return this;
        }

        public FilesystemSearch WithExtensions(params string[] extensions)
            => WithExtensions(extensions as IEnumerable<string>);

        // ---------------
        // Finding results
        // ---------------

        public FilesystemSearchResult FindMatches(string pattern)
        {
            var searchedPaths = GetSearchedPaths();
            return FilesystemPathsMatcher.MatchSearchResult(pattern, searchedPaths);
        }

        public FilesystemSearchResult FindAll()
        {
            var searchedPaths = GetSearchedPaths();
            return new FilesystemSearchResult(searchedPaths, foundPaths: searchedPaths);
        }

        private IReadOnlyCollection<FilePath> GetSearchedPaths()
        {
            var baseDirectory = SearchedDirectoryPath.GetDirectory();
            
            IEnumerable<FileSystemInfo> filesystemItems = IsDirectoriesSearch
                ? baseDirectory.EnumerateDirectories(searchPattern: "*", this.EnumerationOptions)
                : baseDirectory.EnumerateFiles(searchPattern: "*", this.EnumerationOptions);

            var filePaths = filesystemItems.Select(item => FilePath.From(item.FullName));
            if (AllowedExtensions != null)
            {
                filePaths = filePaths.Where(path => AllowedExtensions.Contains(path.GetExtension()));
            }
            return filePaths.ToList();
        }
    }
}
