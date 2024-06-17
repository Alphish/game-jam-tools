using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTally.Model.Vote.Serialization
{
    internal class JamVoteContent
    {
        public string Header { get; }
        public string Version { get; }
        public IReadOnlyCollection<JamVoteSection> Sections { get; }

        private JamVoteContent(string header, string version, IEnumerable<JamVoteSection> sections)
        {
            Header = header;
            Version = version;
            Sections = sections.ToList();
        }

        public static JamVoteContent CreateForHeader(string header, IEnumerable<JamVoteSection> sections)
        {
            var version = ExtractVersion(header);
            return new JamVoteContent(header, version, sections);
        }

        public static JamVoteContent CreateForVersion(string version, IEnumerable<JamVoteSection> sections)
        {
            var header = WrapVersion(version);
            return new JamVoteContent(header, version, sections);
        }

        // ---------------
        // Header handling
        // ---------------

        public static bool IsValidHeader(string header)
        {
            if (!header.StartsWith("###") || !header.EndsWith("###"))
                return false;

            var innerHeader = header
                .Remove(header.Length - "###".Length)
                .Substring("###".Length)
                .Trim();

            if (!innerHeader.StartsWith("VOTE", StringComparison.OrdinalIgnoreCase))
                return false;

            innerHeader = innerHeader.Substring("VOTE".Length);
            if (!string.IsNullOrWhiteSpace(innerHeader) && innerHeader.TrimStart() == innerHeader)
                return false;

            return true;
        }

        private static string ExtractVersion(string header)
        {
            if (!IsValidHeader(header))
                throw new ArgumentException($"Invalid Jam vote header format '{header}'.", nameof(header));
            
            var version = header
                .Remove(header.Length - "###".Length)
                .Substring("###".Length)
                .Trim()
                .Substring("VOTE".Length)
                .Trim();

            return version;
        }

        private static string WrapVersion(string version)
            => !string.IsNullOrWhiteSpace(version) ? $"### VOTE {version} ###" : "### VOTE ###";
    }
}
