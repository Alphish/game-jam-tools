using System.Collections.Generic;
using System.Linq;

using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam;

using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamPlayer.Model.Ratings;
using Alphicsh.JamPlayer.Model.Ratings.NumericScale;

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

        public void LoadJamFromFile(FilePath jamFilePath)
        {
            var jamInfo = JamInfo.LoadFromFile(jamFilePath);
            if (jamInfo == null)
                return;

            var mapper = new JamInfoMapper();
            var jam = mapper.MapInfoToJam(jamInfo);
            LoadJam(jam);
        }

        public void LoadJam(JamOverview jam)
        {
            Jam = jam;
            Ranking = new RankingOverview();

            Ranking.PendingEntries = jam.Entries
                .Select(CreateRankingEntryFromJamEntry)
                .ToList();
        }

        private RankingEntry CreateRankingEntryFromJamEntry(JamEntry jamEntry)
        {
            var defaultSkin = new NumericScaleMaskSkin
            {
                Id = "NumericScaleDefault",
                BackgroundMaskKey = "StarEmptySource",
                ForegroundMaskKey = "StarFullSource",
                ActiveBrushKey = "HighlightText",
                NoValueBrushKey = "ExtraDimText"
            };
            var defaultScaleOptions = new NumericScaleOptions { MaxValue = 5, ValueStep = 0.5, Skin = defaultSkin };

            return new RankingEntry
            {
                JamEntry = jamEntry,
                Ratings = new List<IRating>
                {
                    new NumericScaleRating { Id = "theme", Name = "Theme", Options = defaultScaleOptions },
                    new NumericScaleRating { Id = "concept", Name = "Concept", Options = defaultScaleOptions },
                    new NumericScaleRating { Id = "presentation", Name = "Presentation", Options = defaultScaleOptions },
                    new NumericScaleRating { Id = "story", Name = "Story", Options = defaultScaleOptions },
                    new NumericScaleRating { Id = "overall", Name = "Overall", Options = defaultScaleOptions },
                },
            };
        }
    }
}
