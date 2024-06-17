using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTools.Common.IO.Storage
{
    public class FileBatch : IEquatable<FileBatch?>
    {
        public FileData CoreFile { get; }
        public IReadOnlyCollection<FileData> AuxiliaryFiles { get; }
        private IReadOnlyDictionary<FilePath, FileData> FilesByPath { get; }

        internal string Qualifier { get; }

        public FileBatch(FileData coreFile, IEnumerable<FileData> auxiliaryFiles)
        {
            CoreFile = coreFile;
            AuxiliaryFiles = auxiliaryFiles.OrderBy(file => file.Path.Value).ToList();
            
            FilesByPath = GetFiles().ToDictionary(file => file.Path);

            Qualifier = string.Join("\n", GetFiles().Select(file => file.Qualifier));
        }

        public IEnumerable<FileData> GetFiles()
        {
            yield return CoreFile;

            foreach (var file in AuxiliaryFiles)
            {
                yield return file;
            }
        }

        public FileData? GetFileAt(FilePath path)
        {
            return FilesByPath.TryGetValue(path, out var result) ? result : null;
        }

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
            => Equals(obj as FileBatch);

        public bool Equals(FileBatch? other)
            => other is not null && GetFiles().SequenceEqual(other.GetFiles());

        public override int GetHashCode()
            => Qualifier.GetHashCode();
    }
}
