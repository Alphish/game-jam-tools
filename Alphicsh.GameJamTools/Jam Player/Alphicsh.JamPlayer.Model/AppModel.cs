using System;
using System.Collections.Generic;
using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.Model.Export;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.Model.Jam.Loading;
using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamPlayer.Model.Ratings;
using Alphicsh.JamPlayer.Model.Ratings.NumericScale;
using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model
{
    public class AppModel
    {
        internal static AppModel Current { get; set; } = default!;

        public JamOverview Jam { get; private set; }
        public JamPlayerDataManager PlayerDataManager { get; }

        public RatingCriteriaOverview RatingCriteria { get; internal set; }
        public RankingOverview Ranking { get; internal set; }
        public AwardsOverview Awards { get; internal set; }

        public Exporter Exporter { get; internal set; }

        // -----
        // Setup
        // -----

        public AppModel()
        {
            if (Current != null)
                throw new InvalidOperationException("AppModel should be created only once.");

            Current = this;

            Jam = new JamOverview
            {
                Entries = new List<JamEntry>(),
                AwardCriteria = new List<JamAwardCriterion>()
            };
            PlayerDataManager = new JamPlayerDataManager { AppModel = this };

            RatingCriteria = CreateDefaultRatingCriteria();
            Ranking = new RankingOverview();
            Awards = new AwardsOverview();

            Exporter = new Exporter(this);
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
                CreateNumericScaleCriterion(id: "story", name: "Story", defaultNumericScaleSkin),
                CreateNumericScaleCriterion(id: "visuals", name: "Visuals", defaultNumericScaleSkin),
                CreateNumericScaleCriterion(id: "audio", name: "Audio", defaultNumericScaleSkin),
                CreateNumericScaleCriterion(id: "ux", name: "UX", defaultNumericScaleSkin),
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

        private static JamLoader JamLoader { get; } = new JamLoader();

        public void LoadJamFromFile(FilePath jamFilePath)
        {
            Jam = JamLoader.ReadFromDirectory(jamFilePath.GetParentDirectoryPath())!;
            PlayerDataManager.LoadRanking();
            PlayerDataManager.LoadExporter();
        }
    }
}
