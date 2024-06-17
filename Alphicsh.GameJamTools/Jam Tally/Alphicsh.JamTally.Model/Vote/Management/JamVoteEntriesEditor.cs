using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote.Search;

namespace Alphicsh.JamTally.Model.Vote.Management
{
    public class JamVoteEntriesEditor
    {
        public JamVoteEntriesEditor(JamVote vote)
        {
            Vote = vote;
            Jam = JamTallyModel.Current.Jam!;
            JamSearch = Jam.Search!;

            AuthoredEntries = Vote.Authored.ToList();
            RankingEntries = Vote.Ranking.ToList();
            UnjudgedEntries = Vote.Unjudged.ToList();
            Awards = Vote.Awards.ToList();
            DirectReviewsCount = Vote.DirectReviewsCount;
            ReviewedEntries = Vote.ReviewedEntries.ToList();

            IsReady = true;
            HasErrors = true;
            Output = "All OK!\n";
            FormatText();
        }

        private JamVote Vote { get; }
        private JamOverview Jam { get; }
        private JamSearch JamSearch { get; }

        // ----------------
        // Processing state
        // ----------------

        public bool IsReady { get; private set; }
        public bool HasErrors { get; private set; }
        public string Output { get; private set; }

        private void Modify(Action modification)
        {
            modification();
            IsReady = false;
            HasErrors = false;
        }

        private TResult? WriteOutput<TResult>(string output)
        {
            Output += output + "\n";
            return default;
        }

        private TResult? WriteError<TResult>(string error)
        {
            HasErrors = true;
            return WriteOutput<TResult>(error);
        }

        // -------------
        // Editable text
        // -------------

        private string UnderlyingAuthoredText { get; set; } = default!;
        public string AuthoredText
        {
            get => UnderlyingAuthoredText;
            set => Modify(() => UnderlyingAuthoredText = value);
        }

        private string UnderlyingRankingText { get; set; } = default!;
        public string RankingText
        {
            get => UnderlyingRankingText;
            set => Modify(() => UnderlyingRankingText = value);
        }

        private string UnderlyingUnjudgedText { get; set; } = default!;
        public string UnjudgedText
        {
            get => UnderlyingUnjudgedText;
            set => Modify(() => UnderlyingUnjudgedText = value);
        }

        private string UnderlyingAwardsText { get; set; } = default!;
        public string AwardsText
        {
            get => UnderlyingAwardsText;
            set => Modify(() => UnderlyingAwardsText = value);
        }

        private string UnderlyingReviewedText { get; set; } = default!;
        public string ReviewedText
        {
            get => UnderlyingReviewedText;
            set => Modify(() => UnderlyingReviewedText = value);
        }

        // ---------
        // Vote data
        // ---------

        public IReadOnlyCollection<JamEntry> AuthoredEntries { get; private set; }
        public IReadOnlyCollection<JamEntry> RankingEntries { get; private set; }
        public IReadOnlyCollection<JamEntry> UnjudgedEntries { get; private set; }

        public IReadOnlyCollection<JamVoteAward> Awards { get; private set; }

        public int? DirectReviewsCount { get; private set; }
        public IReadOnlyCollection<JamEntry> ReviewedEntries { get; private set; }

        // ----------
        // Processing
        // ----------

        public void ProcessInputs()
        {
            IsReady = true;
            HasErrors = false;
            Output = string.Empty;

            AuthoredEntries = ReadEntries(AuthoredText, unordered: true);
            RankingEntries = ReadEntries(RankingText, unordered: false);
            UnjudgedEntries = ReadEntries(UnjudgedText, unordered: true).Except(AuthoredEntries).ToList();
            EnsureNoDuplicateEntries();

            Awards = ReadAwards(AwardsText);

            ProcessReviewed(ReviewedText);

            if (!HasErrors)
            {
                WriteOutput<object>("All OK!");
                FormatText();
            }
        }

        private IReadOnlyCollection<string> GetLines(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
        }

        // Entries

        private IReadOnlyCollection<JamEntry> ReadEntries(string text, bool unordered)
        {
            var lines = GetLines(text);
            var result = new List<JamEntry>();
            foreach (var line in lines)
            {
                var entry = TryReadEntry(line);
                if (entry == null)
                    continue;

                if (result.Contains(entry))
                {
                    if (!unordered)
                        WriteError<JamEntry>($"Duplicate ranking entry for '{line}'.");

                    continue;
                }

                result.Add(entry);
            }

            if (unordered)
                result = result.OrderBy(entry => entry.FullLine).ToList();

            return result;
        }

        private JamEntry? TryReadEntry(string line)
        {
            var entry = JamSearch.FindEntry(line, unprefixRanking: true);
            if (entry == null)
                return WriteError<JamEntry>($"Could not resolve entry for '{line}'.");

            return entry;
        }

        private void EnsureNoDuplicateEntries()
        {
            var joinedEntries = AuthoredEntries.Concat(RankingEntries).Concat(UnjudgedEntries);
            var duplicateEntries = joinedEntries
                .GroupBy(entry => entry)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            foreach (var entry in duplicateEntries)
            {
                WriteError<JamEntry>($"Duplicate entry found between ranking/authored/unranked: '{entry.FullLine}'.");
            }
        }

        // Awards

        private IReadOnlyCollection<JamVoteAward> ReadAwards(string text)
        {
            var lines = GetLines(text);
            var awardsByCriterion = new Dictionary<JamAwardCriterion, JamVoteAward>();
            foreach (var line in lines)
            {
                var award = TryReadAward(line);
                if (award == null)
                    continue;

                if (awardsByCriterion.ContainsKey(award.Criterion))
                {
                    WriteError<JamVoteAward>($"Duplicate {award.Criterion.Name} nomination found in '{line}'.");
                    continue;
                }

                awardsByCriterion.Add(award.Criterion, award);
            }

            return Jam.AwardCriteria
                .Where(criterion => awardsByCriterion.ContainsKey(criterion))
                .Select(criterion => awardsByCriterion[criterion])
                .ToList();
        }

        private JamVoteAward? TryReadAward(string line)
        {
            if (!JamSearch.IsAwardWellFormed(line))
                return WriteError<JamVoteAward>($"Expected a line like '<award>: <entry>', got '{line}' instead.");

            var criterion = JamSearch.FindAwardCriterion(line);
            if (criterion == null)
                return WriteError<JamVoteAward>($"Could not resolve award criterion for '{line}'.");

            var entry = JamSearch.FindAwardEntry(line);
            if (entry == null)
                return WriteError<JamVoteAward>($"Could not resolve award entry for '{line}'.");

            return new JamVoteAward { Criterion = criterion, Entry = entry };
        }

        // Reviewed

        private void ProcessReviewed(string text)
        {
            var lines = GetLines(text);
            int? directCount = null;
            var entries = new List<JamEntry>();
            foreach (var line in lines)
            {
                if (line.StartsWith("="))
                {
                    var newCount = TryParseDirectCount(line);
                    if (newCount == null)
                        continue;

                    if (directCount != null)
                        WriteError<int?>($"Duplicate direct reviews count entry '{line}'.");
                    else
                        directCount = newCount;

                    continue;
                }

                var entry = TryReadEntry(line);
                if (entry == null)
                    continue;

                entries.Add(entry);
            }

            DirectReviewsCount = directCount;
            ReviewedEntries = entries.Distinct().OrderBy(entry => entry.FullLine).ToList();
        }

        private int? TryParseDirectCount(string line)
        {
            var directCountString = line.Substring(1).Trim();
            if (!int.TryParse(directCountString, out var parsedCount))
                return WriteError<int?>($"Could not resolve direct reviews count for '{line}'.");

            return parsedCount;
        }

        // ----------
        // Formatting
        // ----------

        public void FormatText()
        {
            UnderlyingAuthoredText = FormatEntries(AuthoredEntries);
            UnderlyingRankingText = FormatEntries(RankingEntries);
            UnderlyingUnjudgedText = FormatEntries(UnjudgedEntries);
            UnderlyingAwardsText = FormatAwards(Awards);
            UnderlyingReviewedText = FormatReviewed(DirectReviewsCount, ReviewedEntries);
        }

        private string FormatEntries(IReadOnlyCollection<JamEntry> entries)
        {
            var lines = entries.Select(entry => entry.FullLine);
            return string.Join("\n", lines);
        }

        private string FormatAwards(IReadOnlyCollection<JamVoteAward> awards)
        {
            var lines = awards.Select(award => award.FullLine);
            return string.Join("\n", lines);
        }

        private string FormatReviewed(int? directCount, IReadOnlyCollection<JamEntry> entries)
        {
            var lines = entries.Select(entry => entry.FullLine).ToList();
            if (directCount.HasValue)
                lines.Insert(0, $"={directCount}");

            return string.Join("\n", lines);
        }

        // ----------
        // Confirming
        // ----------

        public bool CanUpdateVote()
            => IsReady && !HasErrors;

        public void UpdateVote()
        {
            Vote.UpdateRankableEntries(Jam.Entries, AuthoredEntries, RankingEntries, UnjudgedEntries);
            Vote.Awards = Awards;
            Vote.DirectReviewsCount = DirectReviewsCount;
            Vote.SetReviewedEntries(ReviewedEntries);
        }
    }
}
