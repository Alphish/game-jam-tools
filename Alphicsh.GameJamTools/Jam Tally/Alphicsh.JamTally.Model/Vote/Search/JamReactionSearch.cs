using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote.Search
{
    internal class JamReactionSearch
    {
        private IReadOnlyDictionary<string, JamReactionType> ReactionTypesByName { get; }
        private IReadOnlyDictionary<int, JamReactionType> ReactionTypesByValue { get; }

        public JamReactionSearch(JamOverview jam)
        {
            ReactionTypesByName = MakeReactionTypesByName(jam);
            ReactionTypesByValue = MakeReactionTypesByValue(jam);
        }

        // -----
        // Setup
        // -----

        private IReadOnlyDictionary<string, JamReactionType> MakeReactionTypesByName(JamOverview jam)
        {
            return jam.ReactionTypes.ToDictionary(type => type.Name, StringComparer.OrdinalIgnoreCase);
        }

        private IReadOnlyDictionary<int, JamReactionType> MakeReactionTypesByValue(JamOverview jam)
        {
            return jam.ReactionTypes
                .Where(type => type.IsPrimary)
                .ToDictionary(type => type.Value);
        }

        // ------
        // Search
        // ------

        public bool IsReactionWellFormed(string line)
        {
            line = line.Trim();
            return line.Contains(" ");
        }

        public JamReactionType? FindReactionType(string line)
        {
            line = line.Trim();
            var spaceIdx = line.IndexOf(" ");
            var typePart = line.Remove(spaceIdx).Trim();
            return FindReactionTypeByName(typePart) ?? FindReactionTypeByValue(typePart);
        }

        private JamReactionType? FindReactionTypeByName(string typePart)
        {
            return ReactionTypesByName.TryGetValue(typePart, out var type) ? type : null;
        }

        private JamReactionType? FindReactionTypeByValue(string typePart)
        {
            typePart = typePart.TrimStart('+');
            if (!int.TryParse(typePart, out var typeValue))
                return null;

            return ReactionTypesByValue.TryGetValue(typeValue, out var type) ? type : null;
        }

        public string? FindReactionUser(string line)
        {
            line = line.Trim();
            var spaceIdx = line.IndexOf(" ");
            var userPart = line.Substring(spaceIdx).Trim();
            return userPart;
        }

        public JamVoteReaction? FindReaction(string line)
        {
            if (!IsReactionWellFormed(line))
                return null;

            var reactionType = FindReactionType(line);
            var user = FindReactionUser(line);
            if (reactionType == null || user == null)
                return null;

            return new JamVoteReaction { Type = reactionType, User = user };
        }
    }
}
