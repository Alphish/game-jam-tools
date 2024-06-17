using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTally.Model.Vote.Serialization
{
    public class JamVoteSection
    {
        public string Header { get; }
        public string Title { get; }
        public IReadOnlyList<string> Lines { get; }
        public int Length => Lines.Count;

        private JamVoteSection(string header, string title, IEnumerable<string> lines)
        {
            Header = header;
            Title = title;
            Lines = lines.ToList();
        }

        public static JamVoteSection CreateForHeader(string header, IEnumerable<string> lines)
        {
            var title = ExtractTitle(header);
            return new JamVoteSection(header, title, lines);
        }

        public static JamVoteSection CreateForTitle(string title, IEnumerable<string> lines)
        {
            var header = WrapTitle(title);
            return new JamVoteSection(header, title, lines);
        }

        // ---------------
        // Header handling
        // ---------------

        public static bool IsValidHeader(string header)
        {
            if (!header.StartsWith("#"))
                return false;

            return true;
        }

        private static string ExtractTitle(string header)
        {
            if (!IsValidHeader(header))
                throw new ArgumentException($"Invalid Jam vote section header format '{header}'.", nameof(header));

            return header.TrimStart('#').Trim();
        }

        private static string WrapTitle(string title)
            => $"# {title}";
    }
}
