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

        public RatingCriteriaOverview RatingCriteria { get; internal set; }
        public RankingOverview Ranking { get; internal set; }

        // -----
        // Setup
        // -----

        public AppModel()
        {
            Jam = new JamOverview { Entries = new List<JamEntry>() };
            Ranking = new RankingOverview();
            RatingCriteria = CreateDefaultRatingCriteria();
        }

        private RatingCriteriaOverview CreateDefaultRatingCriteria()
        {
            var defaultNumericScaleSkin = new NumericScaleMaskSkin
            {
                Id = "NumericScaleDefault",
                BackgroundMaskKey = "StarEmptySource",
                ForegroundMaskKey = "StarFullSource",
                ActiveBrushKey = "HighlightText",
                NoValueBrushKey = "ExtraDimText"
            };

            var ratingCriteria = new List<IRatingCriterion>
            {
                CreateNumericScaleCriterion(id: "theme", name: "Theme", defaultNumericScaleSkin),
                CreateNumericScaleCriterion(id: "concept", name: "Concept", defaultNumericScaleSkin),
                CreateNumericScaleCriterion(id: "presentation", name: "Presentation", defaultNumericScaleSkin),
                CreateNumericScaleCriterion(id: "story", name: "Story", defaultNumericScaleSkin),
                CreateNumericScaleCriterion(id: "overall", name: "Overall", defaultNumericScaleSkin),
            };

            return new RatingCriteriaOverview
            {
                Skins = new IRatingSkin[] { defaultNumericScaleSkin },
                Criteria = ratingCriteria,
            };
        }

        private NumericScaleCriterion CreateNumericScaleCriterion(string id, string name, INumericScaleSkin skin)
        {
            return new NumericScaleCriterion
            {
                Id = id,
                Name = name,
                MaxValue = 5,
                ValueStep = 1,
                Skin = skin,
            };
        }

        // -----------
        // Loading Jam
        // -----------

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
            return new RankingEntry
            {
                JamEntry = jamEntry,
                Ratings = RatingCriteria.Criteria.Select(criterion => criterion.CreateRating()).ToList(),
            };
        }
    }
}
