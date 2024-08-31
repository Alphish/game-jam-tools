using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.Model.Export;
using Alphicsh.JamPlayer.Model.Feedback;
using Alphicsh.JamPlayer.Model.Feedback.Loading;
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

        private JamLoadModel JamLoadModel { get; }

        public JamOverview Jam => JamLoadModel.Model!;
        public JamPlayerDataManager PlayerDataManager { get; }

        public RatingCriteriaOverview RatingCriteria { get; internal set; }

        public FeedbackLoadModel FeedbackLoadModel { get; }
        public JamFeedback Feedback => FeedbackLoadModel.Model!;
        public RankingOverview Ranking => Feedback.Ranking;
        public AwardsOverview Awards => Feedback.Awards;

        public Exporter Exporter { get; internal set; }

        // -----
        // Setup
        // -----

        public AppModel()
        {
            if (Current != null)
                throw new InvalidOperationException("AppModel should be created only once.");

            Current = this;

            JamLoadModel = new JamLoadModel();
            RatingCriteria = CreateDefaultRatingCriteria();
            FeedbackLoadModel = new FeedbackLoadModel(Jam, RatingCriteria);

            PlayerDataManager = new JamPlayerDataManager { AppModel = this };

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

        public async Task LoadJamFromFile(FilePath jamFilePath)
        {
            await JamLoadModel.LoadFrom(jamFilePath);

            FeedbackLoadModel.UpdateContext(Jam, RatingCriteria);
            await FeedbackLoadModel.LoadFrom(Jam.DirectoryPath);
            
            PlayerDataManager.LoadExporter();
        }

        public async void ResetUserData()
        {
            var userDataPath = Jam.DirectoryPath.Append(".jamplayer");
            
            Directory.Delete(userDataPath.Value, recursive: true);
            await FeedbackLoadModel.LoadFrom(Jam.DirectoryPath);
            PlayerDataManager.LoadExporter();
        }
    }
}
