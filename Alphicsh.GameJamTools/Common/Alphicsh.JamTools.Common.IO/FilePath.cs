using System;
using System.IO;

namespace Alphicsh.JamTools.Common.IO
{
    public struct FilePath
    {
        public string Value { get; }

        public FilePath(string path)
        {
            // store path normalised to the same directory separator char
            Value = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        public static FilePath From(string path)
            => new FilePath(path);

        public static FilePath? FromNullable(string? path)
            => path != null ? new FilePath(path) : (FilePath?)null;

        public override string ToString()
        {
            return Value;
        }

        // -----------------------
        // Absolute/relative paths
        // -----------------------

        public bool IsAbsolute()
            => Path.IsPathRooted(Value);
        public bool IsRelative()
            => !IsAbsolute();
        public FilePath ToFullPath()
            => IsRelative() ? new FilePath(Path.GetFullPath(Value)) : this;
        
        // ----------------
        // Paths operations
        // ----------------
        
        public FilePath AsRelativeTo(FilePath ancestorPath)
        {
            if (IsRelative() || ancestorPath.IsRelative())
                throw new InvalidOperationException("To obtain a path relative to the other, both starting paths must be absolute.");

            var ownFullPath = Path.GetFullPath(Value);
            var ancestorFullPath = Path.GetFullPath(ancestorPath.Value);

            if (!ownFullPath.StartsWith(ancestorFullPath))
                throw new ArgumentException("The given path is not an ancestor to the calling path.", nameof(ancestorPath));

            var relativePathValue = ownFullPath.Substring(ancestorFullPath.Length).TrimStart(Path.DirectorySeparatorChar);
            return FilePath.From(relativePathValue);
        }

        public FilePath Append(string relativePath)
        {
            if (IsRelative())
                throw new InvalidOperationException("Only an absolute path can be appended to.");

            var pathString = Path.GetFullPath(relativePath, this.Value);
            return FilePath.From(pathString);
        }

        public FilePath Append(FilePath relativePath)
            => Append(relativePath.Value);

        public FilePath? AppendNullable(string? relativePath)
            => relativePath != null ? Append(relativePath) : null;

        public FilePath? AppendNullable(FilePath? relativePath)
            => relativePath != null ? Append(relativePath.Value) : null;

        // -------------------
        // File/directory info
        // -------------------

        public Uri ToUri()
            => new Uri(this.Value);

        public bool HasFile()
            => File.Exists(Value);
        public FileInfo GetFile()
            => new FileInfo(Value);
        public bool HasDirectory()
            => Directory.Exists(Value);
        public DirectoryInfo GetDirectory()
            => new DirectoryInfo(Value);

        public FilePath? GetParentDirectoryPath()
            => FilePath.FromNullable(Path.GetDirectoryName(Value));
        public string GetLastSegmentName()
            => Path.GetFileName(this.Value);
        public string GetExtension()
            => Path.GetExtension(this.Value);
    }
}
