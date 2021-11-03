using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam;

using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.Model
{
    public class AppModel
    {
        public JamOverview Jam { get; private set; }
        public RankingOverview Ranking { get; private set; }

        public AppModel()
        {
            Jam = new JamOverview { Entries = new List<JamEntry>() };
            Ranking = new RankingOverview();
        }

        public void LoadJamFromDirectory(FilePath jamDirectoryPath)
        {
            var jamInfo = JamInfo.LoadFromDirectory(jamDirectoryPath);
            var mapper = new JamInfoMapper();
            var jam = mapper.MapInfoToJam(jamInfo);
            LoadJam(jam);
        }

        public void LoadJam(JamOverview jam)
        {
            Jam = jam;
            Ranking = new RankingOverview();

            // TODO: add pending entries to randomly pick the next entry from
            Ranking.UnrankedEntries = jam.Entries
                .Select(entry => new RankingEntry { JamEntry = entry })
                .ToList();
        }
    }
}
