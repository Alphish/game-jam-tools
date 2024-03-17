using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTally.Model.Vote.Search;

namespace Alphicsh.JamTally.Model.Vote.Serialization.Parsing
{
    internal abstract class JamVoteParserBase
    {
        public abstract string Version { get; }

        protected JamVoteContent Content { get; }
        protected JamOverview Jam { get; }
        protected JamSearch JamSearch { get; }

        public JamVote Vote { get; private set; } = default!;

        protected JamVoteParserBase(JamVoteContent content, JamOverview jam, JamSearch jamSearch)
        {
            Content = content;
            Jam = jam;
            JamSearch = jamSearch;
        }

        public void Process() => Vote = Parse();
        public abstract JamVote Parse();

        // -----
        // Voter
        // -----

        protected void ProcessVoterSection(JamVote vote, JamVoteSection section)
        {
            if (section.Length >= 1)
            {
                vote.Voter = section.Lines[0].Trim();
            }

            if (section.Length >= 2)
            {
                if (!JamSearch.AlignmentsEnabled)
                    throw new InvalidOperationException($"There are no alignments specified for this Jam, and thus voter alignment is not applicable.");

                var line = section.Lines[1];
                if (!JamSearch.IsAlignmentValid(line))
                    throw new InvalidOperationException($"Could not resolve alignment for '{line}'.");

                vote.Alignment = JamSearch.FindAlignment(line);
            }

            if (section.Length > 2)
                throw new InvalidOperationException($"The voter section should only have the voter line and alignment line.");
        }

        // -------
        // Entries
        // -------

        protected IReadOnlyCollection<JamEntry> ParseEntriesSection(JamVoteSection section, bool unordered)
        {
            var entries = section.Lines.Select(ParseEntryLine);
            if (unordered)
                entries = entries.OrderBy(entry => entry.FullLine);

            return entries.ToList();
        }

        private JamEntry ParseEntryLine(string line)
        {
            var entry = JamSearch.FindEntry(line, unprefixRanking: false);
            if (entry == null)
                throw new InvalidOperationException($"Could not resolve entry line '{line}'.");

            return entry;
        }

        // ------
        // Awards
        // ------

        protected IReadOnlyCollection<JamVoteAward> ParseAwardsSection(JamVoteSection section)
        {
            var awardsLookup = section.Lines.Select(ParseAwardLine).ToLookup(award => award.Criterion);

            var awards = new List<JamVoteAward>();
            foreach (var criterion in Jam.AwardCriteria)
            {
                var entries = awardsLookup[criterion].ToList();
                if (entries.Count == 0)
                    continue;
                else if (entries.Count > 1)
                    throw new InvalidOperationException($"Duplicate votes for '{criterion.Name}'.");
                else
                    awards.Add(entries.Single());
            }
            return awards;
        }

        private JamVoteAward ParseAwardLine(string line)
        {
            if (!JamSearch.IsAwardWellFormed(line))
                throw new FormatException($"The award line should be in the '<award>: <entry>' format.");

            var criterion = JamSearch.FindAwardCriterion(line);
            if (criterion == null)
                throw new InvalidOperationException($"Could not resolve award criterion for '{line}'.");

            var entry = JamSearch.FindAwardEntry(line);
            if (entry == null)
                throw new InvalidOperationException($"Could not resolve award entry for '{line}'.");

            return new JamVoteAward { Criterion = criterion, Entry = entry };
        }

        // ---------
        // Reactions
        // ---------

        protected IReadOnlyCollection<JamVoteReaction> ParseReactionsSection(JamVoteSection section)
        {
            return section.Lines.Select(ParseReactionLine).ToList();
        }

        private JamVoteReaction ParseReactionLine(string line)
        {
            if (!JamSearch.IsReactionWellFormed(line))
                throw new FormatException($"The reaction line should be in the '<type> <user>' format.");

            var reactionType = JamSearch.FindReactionType(line);
            if (reactionType == null)
                throw new InvalidOperationException($"Could not resolve reaction type for '{line}'.");

            var user = JamSearch.FindReactionUser(line);
            if (user == null)
                throw new InvalidOperationException($"Could not resolve reaction user for '{line}'.");

            return new JamVoteReaction { Type = reactionType, User = user };
        }
    }
}
