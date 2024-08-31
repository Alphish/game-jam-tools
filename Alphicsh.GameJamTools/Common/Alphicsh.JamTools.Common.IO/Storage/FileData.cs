using System;
using System.Security.Cryptography;
using System.Text;

namespace Alphicsh.JamTools.Common.IO.Storage
{
    public class FileData : IEquatable<FileData?>
    {
        public FilePath Path { get; }
        public string Content { get; }

        internal string Qualifier { get; }
        internal string IntegrityHash { get; }

        public FileData(FilePath path, string content)
        {
            Path = path;
            Content = content;

            Qualifier = $"@@{Path.Value}\n{Content}";
            IntegrityHash = ComputeHash(Content);
        }

        private string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
            => Equals(obj as FileData);

        public bool Equals(FileData? other)
            => other is not null && Qualifier.Equals(other.Qualifier);

        public override int GetHashCode()
            => Qualifier.GetHashCode();
    }
}
