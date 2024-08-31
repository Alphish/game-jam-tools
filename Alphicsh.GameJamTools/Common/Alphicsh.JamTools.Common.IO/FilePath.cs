using System;
using System.IO;
using System.Text.Json.Serialization;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTools.Common.IO
{
    [JsonConverter(typeof(FilePathJsonConverter))]
    public struct FilePath : IEquatable<FilePath>
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

        public static FilePath Of(DirectoryInfo directory)
            => new FilePath(directory.FullName);

        public static FilePath Of(FileInfo fileInfo)
            => new FilePath(fileInfo.FullName);

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

        public bool IsSubpathOf(FilePath ancestorPath)
        {
            if (IsRelative() || ancestorPath.IsRelative())
                return false;

            var ownFullPath = Path.GetFullPath(Value);
            var ancestorFullPath = Path.GetFullPath(ancestorPath.Value);

            return ownFullPath.StartsWith(ancestorFullPath);
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

        public FilePath GetParentDirectoryPath()
        {
            var value = Path.GetDirectoryName(Value);
            if (value == null)
                throw new InvalidOperationException("Cannot get a parent directory path for a root directory.");

            return FilePath.From(value);
        }

        public FilePath ReplaceFilename(string filename)
            => GetParentDirectoryPath().Append(filename);

        // -------------------
        // File/directory info
        // -------------------

        public Uri ToUri()
            => new Uri(this.Value);

        public bool HasFile()
            => File.Exists(Value);
        public bool HasFileOrUpdate()
            => File.Exists(Value) || File.Exists(Value + ".new");
        public FileInfo GetFile()
            => new FileInfo(Value);
        public bool HasDirectory()
            => Directory.Exists(Value);
        public DirectoryInfo GetDirectory()
            => new DirectoryInfo(Value);

        public string GetLastSegmentName()
            => Path.GetFileName(this.Value);
        public string GetNameWithoutExtension()
            => Path.GetFileNameWithoutExtension(this.Value);
        public string GetExtension()
            => Path.GetExtension(this.Value);

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj) => obj is FilePath path && Equals(path);
        public bool Equals(FilePath other) => Value == other.Value;
        public override int GetHashCode() => HashCode.Combine(Value);
        public static bool operator ==(FilePath left, FilePath right) => left.Equals(right);
        public static bool operator !=(FilePath left, FilePath right) => !(left == right);
    }
}
