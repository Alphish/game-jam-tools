using System;
using System.Collections.Generic;
using Alphicsh.JamPlayer.IO.Ranking;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model.Vote.Saving
{
    public class JamVoteSaveData
    {
        public FilePath DirectoryPath { get; init; }
        public JamRankingInfo VoteInfo { get; init; } = default!;

        // --------
        // Equality
        // --------

        public override bool Equals(object? obj)
        {
            return obj is JamVoteSaveData data &&
                   DirectoryPath.Equals(data.DirectoryPath) &&
                   EqualityComparer<JamRankingInfo>.Default.Equals(VoteInfo, data.VoteInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DirectoryPath, VoteInfo);
        }
    }
}
