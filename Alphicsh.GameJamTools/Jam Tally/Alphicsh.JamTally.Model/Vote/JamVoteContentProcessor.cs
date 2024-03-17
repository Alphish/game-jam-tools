using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote.Search;

namespace Alphicsh.JamTally.Model.Vote
{
    internal class JamVoteContentProcessor
    {
        private JamVote Vote { get; }
        private JamSearch JamSearch { get; }
        
        private string? Voter { get; set; }
        private JamAlignmentOption? Alignment { get; set; }

        private int ReviewsCount { get; set; }

        private IDictionary<JamAwardCriterion, JamEntry> Awards { get; } = new Dictionary<JamAwardCriterion, JamEntry>();

        private HashSet<JamEntry> MissingEntries { get; set; } = default!;
        private IList<JamEntry> Ranking { get; } = new List<JamEntry>();
        private IList<JamEntry> UnjudgedEntries { get; } = new List<JamEntry>();
        private IList<JamEntry> AuthoredEntries { get; } = new List<JamEntry>();

        private IList<JamVoteReaction> Reactions { get; } = new List<JamVoteReaction>();

        private string? Error { get; set; }
        private StringBuilder ContentBuilder { get; set; } = new StringBuilder();

        public JamVoteContentProcessor(JamVote vote, JamSearch jamSearch)
        {
            Vote = vote;
            JamSearch = jamSearch;
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
                case "STATS":
                    return ReadStats;
                case "AWARDS":
                    return ReadAward;
                case "RANKING":
                    return ReadRankingEntry;
                case "UNJUDGED":
                    return ReadUnjudgedEntry;
                case "AUTHORED":
                    return ReadAuthoredEntry;
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
            if (Voter == null)
                Voter = line;
            else if (!JamSearch.AlignmentsEnabled)
                throw new InvalidOperationException($"There are no alignments specified for this Jam, and thus voter alignment can't be provided.");
            else if (Alignment != null)
                throw new InvalidOperationException($"Voter name and alignment were already provided.");
            else if (!JamSearch.IsAlignmentValid(line))
                throw new InvalidOperationException($"Could not resolve alignment '{line}'.");
            else
                Alignment = JamSearch.FindAlignment(line);
        }

        private void ReadStats(string line, JamOverview jam)
        {
            line = line.ToLowerInvariant();
            if (line.StartsWith("reviews count:"))
            {
                var countString = line.Substring("reviews count:".Length).Trim();
                ReviewsCount = int.Parse(countString);
            }
            else
            {
                throw new InvalidOperationException($"Cannot read statistics line: '{line}'.");
            }
        }

        // -------
        // Entries
        // -------

        private void ReadAward(string line, JamOverview jam)
        {
            if (!JamSearch.IsAwardWellFormed(line))
                throw new ArgumentException($"The award line '{line}' must be in the 'award: entry' format.", nameof(line));

            var criterion = JamSearch.FindAwardCriterion(line);
            if (criterion == null)
                throw new InvalidOperationException($"Could not resolve award for '{line}'.");

            if (Awards.ContainsKey(criterion))
                throw new InvalidOperationException($"Duplicate award entry for '{criterion.Name}'.");

            var entry = JamSearch.FindAwardEntry(line);
            if (entry == null)
                throw new InvalidOperationException($"Could not resolve award entry for '{line}'.");

            Awards.Add(criterion, entry);
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

        private void ReadAuthoredEntry(string line, JamOverview jam)
        {
            var entry = FindEntryByLine(line, line, jam);
            AuthoredEntries.Add(entry);
        }

        private JamEntry FindEntryByLine(string line, string displayLine, JamOverview jam)
        {
            var entry = JamSearch.FindEntry(line, unprefixRanking: true);
            if (entry == null)
                throw new InvalidOperationException($"Could not resolve entry for line '{displayLine}'.");

            return entry;
        }

        // ---------
        // Reactions
        // ---------

        private void ReadReaction(string line, JamOverview jam)
        {
            var reaction = JamSearch.FindReaction(line);
            if (reaction == null)
                throw new InvalidOperationException($"Could not resolve reaction for line '{line}'.");

            Reactions.Add(reaction);
        }

        // ------------------
        // Content generation
        // ------------------

        private void GenerateContent()
        {
            GenerateNameSection();
            GenerateStatsSection();
            GenerateAwardsSection();
            GenerateRankingSection();
            GenerateUnjudgedEntriesSection();
            GenerateAuthoredEntriesSection();
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
            if (Alignment != null)
                AddLine(Alignment.ShortTitle);
            
            AddLine();
        }

        private void GenerateStatsSection()
        {
            AddSection("STATS");

            if (ReviewsCount != 0)
                AddLine($"Reviews count: {ReviewsCount}");

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

        private void GenerateAuthoredEntriesSection()
        {
            AddSection("AUTHORED");
            foreach (var entry in AuthoredEntries.OrderBy(entry => entry.Line.ToLowerInvariant()))
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
            Vote.Alignment = Alignment;
            Vote.DirectReviewsCount = ReviewsCount;
            Vote.Awards = Awards.Select(kvp => new JamVoteAward { Criterion = kvp.Key, Entry = kvp.Value }).ToList();
            Vote.Ranking = Ranking.ToList();
            Vote.Unjudged = UnjudgedEntries.OrderBy(entry => entry.Line, StringComparer.OrdinalIgnoreCase).ToList();
            Vote.Missing = MissingEntries.OrderBy(entry => entry.Line, StringComparer.OrdinalIgnoreCase).ToList();
            Vote.Authored = AuthoredEntries.OrderBy(entry => entry.Line, StringComparer.OrdinalIgnoreCase).ToList();
            Vote.Reactions = Reactions.ToList();

            Vote.Error = Error;
            if (Error == null)
                Vote.Content = ContentBuilder.ToString();
        }
    }
}
