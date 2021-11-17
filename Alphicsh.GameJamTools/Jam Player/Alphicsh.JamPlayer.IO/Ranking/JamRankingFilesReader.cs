using System;
using System.IO;

using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.IO.Ranking
{
    public class JamRankingFilesReader
    {
        private JsonContentSerializer<JamRankingInfo> JamInfoSerializer { get; }

        public JamRankingFilesReader()
        {
            JamInfoSerializer = new JsonContentSerializer<JamRankingInfo>();
        }

        public JamRankingInfo? TryLoadRankingInfo(FilePath rankingInfoPath)
        {
            if (rankingInfoPath.IsRelative())
                throw new ArgumentException("The jam info can only be read from the absolute file path.", nameof(rankingInfoPath));

            if (!rankingInfoPath.HasFile())
                return null;

            var content = File.ReadAllText(rankingInfoPath.Value);
            var rankingInfo = JamInfoSerializer.Deserialize(content);
            if (rankingInfo == null)
                return null;

            return rankingInfo;
        }
    }
}
