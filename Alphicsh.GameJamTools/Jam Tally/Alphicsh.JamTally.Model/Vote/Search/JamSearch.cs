using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote.Search
{
    internal class JamSearch
    {
        private JamAlignmentSearch AlignmentSearch { get; }
        private JamEntrySearch EntrySearch { get; }
        private JamAwardSearch AwardSearch { get; }
        private JamReactionSearch ReactionSearch { get; }

        public JamSearch(JamOverview jam)
        {
            AlignmentSearch = new JamAlignmentSearch(jam);
            EntrySearch = new JamEntrySearch(jam);
            AwardSearch = new JamAwardSearch(jam, EntrySearch);
            ReactionSearch = new JamReactionSearch(jam);
        }

        // ----------
        // Alignments
        // ----------

        public bool AlignmentsEnabled => AlignmentSearch.AlignmentsEnabled;
        
        public bool IsAlignmentValid(string line)
            => AlignmentSearch.IsAlignmentValid(line);

        public JamAlignmentOption? FindAlignment(string line)
            => AlignmentSearch.FindAlignment(line);

        // -------
        // Entries
        // -------

        public JamEntry? FindEntry(string line, bool unprefixRanking)
            => EntrySearch.FindEntry(line, unprefixRanking);

        public IReadOnlyCollection<JamEntry> FindEntriesBy(string author)
            => EntrySearch.FindEntriesBy(author);

        // ------
        // Awards
        // ------

        public bool IsAwardWellFormed(string line)
            => AwardSearch.IsAwardWellFormed(line);

        public JamAwardCriterion? FindAwardCriterion(string line)
            => AwardSearch.FindAwardCriterion(line);

        public JamEntry? FindAwardEntry(string line)
            => AwardSearch.FindAwardEntry(line);

        public JamVoteAward? FindAward(string line)
            => AwardSearch.FindAward(line);

        // ---------
        // Reactions
        // ---------

        public bool IsReactionWellFormed(string line)
            => ReactionSearch.IsReactionWellFormed(line);

        public JamReactionType? FindReactionType(string line)
            => ReactionSearch.FindReactionType(line);

        public string? FindReactionUser(string line)
            => ReactionSearch.FindReactionUser(line);

        public JamVoteReaction? FindReaction(string line)
            => ReactionSearch.FindReaction(line);
    }
}
