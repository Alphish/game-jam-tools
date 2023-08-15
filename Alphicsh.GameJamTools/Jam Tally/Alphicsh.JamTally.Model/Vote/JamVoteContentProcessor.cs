using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote
{
    public class JamVoteContentProcessor
    {
        private JamVote Vote { get; }
        
        private string? Voter { get; set; }

        private IDictionary<JamAwardCriterion, JamEntry> Awards { get; } = new Dictionary<JamAwardCriterion, JamEntry>();

        private HashSet<JamEntry> MissingEntries { get; set; } = default!;
        private IList<JamEntry> Ranking { get; } = new List<JamEntry>();
        private IList<JamEntry> UnjudgedEntries { get; } = new List<JamEntry>();

        private IList<JamVoteReaction> Reactions { get; } = new List<JamVoteReaction>();

        private string? Error { get; set; }
        private StringBuilder ContentBuilder { get; set; } = new StringBuilder();

        public JamVoteContentProcessor(JamVote vote)
        {
            Vote = vote;
        }

        public void Process()
        {
            var jam = JamTallyModel.Current.Jam;
            MissingEntries = jam!.Entries.ToHashSet();

            var lines = GetVoteLines();
            try
            {
                ProcessLines(lines, jam);
                GenerateContent();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            UpdateVote();
        }

        private string[] GetVoteLines()
        {
            return Vote.Content
                .Replace("\r", "\n")
                .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        // ----------------
        // Basic processing
        // ----------------

        private void ProcessLines(string[] lines, JamOverview jam)
        {
            Action<string, JamOverview> processor = ProcessInitialLine;
            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    processor = ChooseProcessor(line);
                    continue;
                }

                var fixedLine = line;
                if (fixedLine.StartsWith("\\"))
                    fixedLine = fixedLine.Substring(1).Trim();

                processor(fixedLine, jam);
            }
        }

        private Action<string, JamOverview> ChooseProcessor(string line)
        {
            if (!line.StartsWith("#"))
                throw new ArgumentException("The section line must start with a #.", nameof(line));

            var section = line.Substring(1).Trim().ToUpperInvariant();
            switch (section)
            {
                case "VOTER":
                    return ReadVoterName;
                case "AWARDS":
                    return ReadAward;
                case "RANKING":
                    return ReadRankingEntry;
                case "UNJUDGED":
                    return ReadUnjudgedEntry;
                case "REACTIONS":
                    return ReadReaction;
                default:
                    throw new ArgumentException($"Unknown section name name '{section}'.", nameof(line));
            }
        }

        private void ProcessInitialLine(string line, JamOverview jam)
        {
            throw new InvalidOperationException($"Cannot process the line '{line}' without specifying the section first.");
        }

        private void ReadVoterName(string line, JamOverview jam)
        {
            Voter = line;
        }

        // -------
        // Entries
        // -------

        private void ReadAward(string line, JamOverview jam)
        {
            var colonIdx = line.IndexOf(':');
            if (colonIdx < 0)
                throw new ArgumentException($"The award line '{line}' must be in the 'award: entry' format.", nameof(line));

            var awardName = line.Remove(colonIdx).Trim();
            var award = jam.GetAwardByName(awardName);
            if (award == null)
                throw new InvalidOperationException($"Could not resolve award for name '{awardName}'.");

            if (Awards.ContainsKey(award))
                throw new InvalidOperationException($"Duplicate award entry for '{awardName}'.");

            var entryLine = line.Substring(colonIdx + 1).Trim();
            var entry = FindEntryByLine(entryLine, line, jam);

            Awards.Add(award, entry);
        }

        private void ReadRankingEntry(string line, JamOverview jam)
        {
            var entry = FindEntryByLine(line, line, jam);
            if (!MissingEntries.Remove(entry))
                throw new InvalidOperationException($"The entry '{entry.Line}' was already added to the ranking or unjudged entries.");

            Ranking.Add(entry);
        }

        private void ReadUnjudgedEntry(string line, JamOverview jam)
        {
            var entry = FindEntryByLine(line, line, jam);
            if (!MissingEntries.Remove(entry))
                throw new InvalidOperationException($"The entry '{entry.Line}' was already added to the ranking or unjudged entries.");

            UnjudgedEntries.Add(entry);
        }

        private JamEntry FindEntryByLine(string line, string displayLine, JamOverview jam)
        {
            var byLine = jam.GetEntryByLine(line);
            if (byLine != null)
                return byLine;

            var byTitle = jam.GetEntryByTitle(line);
            if (byTitle != null)
                return byTitle;

            var unprefixedLine = UnprefixDigits(line);
            if (unprefixedLine != null)
            {
                var byUnprefixedLine = jam.GetEntryByLine(unprefixedLine);
                if (byUnprefixedLine != null)
                    return byUnprefixedLine;

                var byUnprefixedTitle = jam.GetEntryByTitle(unprefixedLine);
                if (byUnprefixedTitle != null)
                    return byUnprefixedTitle;
            }

            throw new InvalidOperationException($"Could not resolve entry for line '{displayLine}'.");
        }

        private string? UnprefixDigits(string line)
        {
            var idx = 0;
            var chars = line.ToCharArray();
            while (idx < chars.Length && char.IsDigit(chars[idx]))
                idx++;

            if (idx == 0)
                return null;

            if (chars[idx] == '.')
                idx++;

            return line.Substring(idx).Trim();
        }

        // ---------
        // Reactions
        // ---------

        private void ReadReaction(string line, JamOverview jam)
        {
            line = line.TrimStart('+');
            var value = int.Parse(line.Remove(1));
            var name = line.Substring(1).Trim();
            var reaction = new JamVoteReaction
            {
                Name = name,
                Value = value,
            };

            Reactions.Add(reaction);
        }

        // ------------------
        // Content generation
        // ------------------

        private void GenerateContent()
        {
            GenerateNameSection();
            GenerateAwardsSection();
            GenerateRankingSection();
            GenerateUnjudgedEntriesSection();
            GenerateReactionsSection();
        }

        private void AddSection(string sectionName)
        {
            ContentBuilder.AppendLine("# " + sectionName);
        }

        private void AddLine(string line = "")
        {
            if (line.StartsWith("#"))
                line = "\\" + line;

            ContentBuilder.AppendLine(line);
        }

        private void GenerateNameSection()
        {
            AddSection("VOTER");
            AddLine(Voter ?? "<voter>");
            AddLine();
        }

        private void GenerateAwardsSection()
        {
            AddSection("AWARDS");
            var jam = JamTallyModel.Current.Jam!;
            foreach (var award in jam.AwardCriteria)
            {
                if (!Awards.TryGetValue(award, out var entry))
                    continue;

                AddLine($"{award.Name}: {entry.Line}");
            }
            AddLine();
        }

        private void GenerateRankingSection()
        {
            AddSection("RANKING");
            foreach (var entry in Ranking)
            {
                AddLine(entry.Line);
            }
            AddLine();
        }

        private void GenerateUnjudgedEntriesSection()
        {
            AddSection("UNJUDGED");
            foreach (var entry in UnjudgedEntries.OrderBy(entry => entry.Line.ToLowerInvariant()))
            {
                AddLine(entry.Line);
            }
            AddLine();
        }

        private void GenerateReactionsSection()
        {
            AddSection("REACTIONS");
            foreach (var reaction in Reactions)
            {
                AddLine($"+{reaction.Value} {reaction.Name}");
            }
            AddLine();
        }

        // -------------
        // Vote updating
        // -------------

        private void UpdateVote()
        {
            Vote.Voter = Voter;
            Vote.Awards = Awards.Select(kvp => new JamVoteAward { Award = kvp.Key, Entry = kvp.Value }).ToList();
            Vote.Ranking = Ranking.ToList();
            Vote.Unjudged = UnjudgedEntries.OrderBy(entry => entry.Line, StringComparer.OrdinalIgnoreCase).ToList();
            Vote.Missing = MissingEntries.OrderBy(entry => entry.Line, StringComparer.OrdinalIgnoreCase).ToList();
            Vote.Reactions = AggregateReactions();

            Vote.Error = Error;
            if (Error == null)
                Vote.Content = ContentBuilder.ToString();
        }

        private IReadOnlyCollection<JamVoteReaction> AggregateReactions()
        {
            return Reactions
                .GroupBy(reaction => reaction.Name, StringComparer.OrdinalIgnoreCase)
                .Select(group => new JamVoteReaction { Name = group.First().Name, Value = group.Max(reaction => reaction.Value) })
                .OrderBy(reaction => reaction.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
