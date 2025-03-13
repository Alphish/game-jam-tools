using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Trophies.Data
{
    public class TrophiesInput
    {
        public IReadOnlyCollection<JamTrophyEntry> Entries { get; init; } = default!;

        public static void Generate()
        {
            var jamPath = JamTallyModel.Current.VotesCollection!.DirectoryPath;
            var tallyPath = jamPath.Append(".jamtally");
            var trophiesPath = tallyPath.Append("trophies.jamtrophies");
            if (trophiesPath.HasFile())
                throw new InvalidOperationException("Cannot generate the trophies file when one is already present.");

            var entries = JamTallyModel.Current.Jam!.Entries;
            var sb = new StringBuilder();
            foreach (var entry in entries)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                sb.AppendLine("@entry          " + entry.FullLine);
                sb.AppendLine("    @id         " + entry.Title.ToLowerInvariant().Replace(" ", "_"));
                sb.AppendLine("    @title      " + entry.Title);
                sb.AppendLine("    @authors    " + entry.Team);
            }

            File.WriteAllText(trophiesPath.Value, sb.ToString());
        }

        public static TrophiesInput Parse()
        {
            var jamPath = JamTallyModel.Current.VotesCollection!.DirectoryPath;
            var tallyPath = jamPath.Append(".jamtally");
            var trophiesPath = tallyPath.Append("trophies.jamtrophies");

            var content = File.ReadAllText(trophiesPath.Value);
            var lines = content
                .Replace("\r\n", "\n").Replace("\r", "\n")
                .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            return FromLines(lines);
        }

        private static TrophiesInput FromLines(IReadOnlyCollection<string> lines)
        {
            var entries = new List<JamTrophyEntry>();
            JamTrophyEntry? currentEntry = null;
            foreach (var line in lines)
            {
                var prefix = GetPrefix(line);
                var core = GetCore(line);

                switch (prefix)
                {
                    case "@entry":
                        if (currentEntry != null)
                            entries.Add(currentEntry);

                        var jamEntry = ReadEntry(core);
                        currentEntry = new JamTrophyEntry(jamEntry);
                        break;

                    case "@id":
                        currentEntry!.Id = core;
                        break;

                    case "@title":
                        currentEntry!.Title = core;
                        break;

                    case "@authors":
                        currentEntry!.Authors = core;
                        break;
                }
            }

            if (currentEntry != null)
                entries.Add(currentEntry);

            entries = entries.OrderBy(entry => entry.Title).ThenBy(entry => entry.Authors).ToList();

            return new TrophiesInput
            {
                Entries = entries
            };
        }

        private static string GetPrefix(string line)
        {
            var idx = line.IndexOf(' ');
            if (idx < 0)
                throw new InvalidOperationException($"Could not read prefix from trophies line '{line}'.");

            return line.Remove(idx).Trim().ToLowerInvariant();
        }

        private static string GetCore(string line)
        {
            var idx = line.IndexOf(' ');
            if (idx < 0)
                throw new InvalidOperationException($"Could not read prefix from trophies line '{line}'.");

            return line.Substring(idx + 1).Trim();
        }

        private static JamEntry ReadEntry(string core)
        {
            var search = JamTallyModel.Current.Jam!.Search!;
            var entry = search.FindEntry(core, false);
            if (entry == null)
                throw new InvalidOperationException($"Could not read trophies entry from '{core}'.");

            return entry;
        }
    }
}
