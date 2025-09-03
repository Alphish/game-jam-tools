using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTally.Model.Result.Export
{
    public class JamTallyResultExporter
    {
        private static JsonFileSaver<JamTallyResultInfo> Saver { get; } = new JsonFileSaver<JamTallyResultInfo>();

        public void ExportResult(FilePath directoryPath, JamTallyNewResult result)
        {
            var jamResultsPath = directoryPath.Append(".jamtally/results.jamresults");
            var jamResultInfo = MapResult(result);
            Saver.Save(jamResultsPath, jamResultInfo);
        }

        private JamTallyResultInfo MapResult(JamTallyNewResult result)
        {
            var ranking = result.Ranking.Select(entry => entry.EntryId).ToList();
            var awards = new List<JamTallyAwardInfo>();
            foreach (var criterion in result.AwardCriteria)
            {
                var winners = result.AwardEntries[criterion].Select(entry => entry.Id).ToList();
                awards.Add(new JamTallyAwardInfo
                {
                    Id = criterion.Id,
                    Name = criterion.Name,
                    Winners = winners,
                    AwardedTo = "entry",
                });
            }

            awards.Add(new JamTallyAwardInfo
            {
                Id = "reviewer",
                Name = "Best Reviewer",
                Winners = result.TopReviewers,
                AwardedTo = "participant",
            });

            return new JamTallyResultInfo { Ranking = ranking, Awards = awards };
        }
    }
}
