using System.IO;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamPlayer.IO.Ranking
{
    public class JamRankingFilesWriter
    {
        private JsonContentSerializer<JamRankingInfo> JamRankingInfoSerializer { get; }

        public JamRankingFilesWriter()
        {
            JamRankingInfoSerializer = new JsonContentSerializer<JamRankingInfo>();
        }

        public void SaveRankingInfo(JamRankingInfo jamRankingInfo, FilePath rankingInfoPath)
        {
            var content = JamRankingInfoSerializer.Serialize(jamRankingInfo);
            var directoryPath = rankingInfoPath.GetParentDirectoryPath();
            Directory.CreateDirectory(directoryPath.Value);
            File.WriteAllText(rankingInfoPath.Value, content);
        }
    }
}
