using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote.Search;

namespace Alphicsh.JamTally.Model.Vote.Management
{
    public class JamVoteReactionsEditor
    {
        public JamVoteReactionsEditor(JamVote vote)
        {
            Vote = vote;
            Jam = JamTallyModel.Current.Jam!;
            JamSearch = Jam.Search!;

            Reactions = Vote.Reactions.ToList();
            AggregateReactions = Vote.AggregateReactions.ToList();

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

        private string UnderlyingReactionsText { get; set; } = default!;
        public string ReactionsText
        {
            get => UnderlyingReactionsText;
            set => Modify(() => UnderlyingReactionsText = value);
        }

        public string AggregateReactionsText { get; private set; } = default!;

        // ---------
        // Vote data
        // ---------

        public IReadOnlyCollection<JamVoteReaction> Reactions { get; private set; }
        public IReadOnlyCollection<JamVoteReaction> AggregateReactions { get; private set; }

        // ----------
        // Processing
        // ----------

        public void ProcessInputs()
        {
            IsReady = true;
            HasErrors = false;
            Output = string.Empty;

            Reactions = ReadReactions(ReactionsText);
            AggregateReactions = Reactions
                .GroupBy(reaction => reaction.User)
                .Select(group => group.OrderByDescending(reaction => reaction.Value).First())
                .OrderBy(reaction => reaction.User)
                .ToList();

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

        private IReadOnlyCollection<JamVoteReaction> ReadReactions(string text)
        {
            var lines = GetLines(text);
            var result = new List<JamVoteReaction>();
            foreach (var line in lines)
            {
                var reaction = TryReadReaction(line);
                if (reaction == null)
                    continue;

                result.Add(reaction);
            }
            return result;
        }

        private JamVoteReaction? TryReadReaction(string line)
        {
            if (!JamSearch.IsReactionWellFormed(line))
                return WriteError<JamVoteReaction>($"Expected a line like '<reaction> <user>', got '{line}' instead.");

            var type = JamSearch.FindReactionType(line);
            if (type == null)
                return WriteError<JamVoteReaction>($"Could not resolve reaction type for '{line}'.");

            var user = JamSearch.FindReactionUser(line);
            if (user == null)
                return WriteError<JamVoteReaction>($"Could not resolve reaction user for '{line}'.");

            return new JamVoteReaction { Type = type, User = user };
        }

        // ----------
        // Formatting
        // ----------

        public void FormatText()
        {
            UnderlyingReactionsText = FormatReactions(Reactions);
            AggregateReactionsText = FormatAggregateReactions(AggregateReactions);
        }

        private string FormatReactions(IReadOnlyCollection<JamVoteReaction> reactions)
        {
            var lines = reactions.Select(reaction => $"{reaction.Type.Name} {reaction.User}");
            return string.Join("\n", lines);
        }

        private string FormatAggregateReactions(IReadOnlyCollection<JamVoteReaction> reactions)
        {
            var lines = reactions.Select(reaction => $"+{reaction.Value} {reaction.User} ({reaction.Type.Name})").ToList();
            lines.Add("=" + reactions.Sum(reaction => reaction.Value));
            return string.Join("\n", lines);
        }

        // ----------
        // Confirming
        // ----------

        public bool CanUpdateVote()
            => IsReady && !HasErrors;

        public void UpdateVote()
        {
            Vote.SetReactions(Reactions);
        }
    }
}
