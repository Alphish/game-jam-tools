using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Trophies
{
    public class TrophiesBuilder
    {
        public XDocument Document { get; }
        public IReadOnlyDictionary<string, XElement> Layers { get; }
        public IReadOnlyCollection<EntryTrophySection> TrophySections { get; }

        public TrophiesBuilder(XDocument document, JamTallyResult tallyResult)
        {
            Document = document;
            Layers = ExtractLayers(document);
            TrophySections = GenerateTrophySections(tallyResult).ToList();
        }

        private IReadOnlyDictionary<string, XElement> ExtractLayers(XDocument document)
        {
            return document.Root!.Elements()
                .Where(element => element.Name.LocalName == "g")
                .ToDictionary(element => element.InkAttribute("label")!.Value, element => element, StringComparer.OrdinalIgnoreCase);
        }

        private IEnumerable<EntryTrophySection> GenerateTrophySections(JamTallyResult tallyResult)
        {
            var rankSections = new Dictionary<JamEntry, EntryTrophySection>();

            // generate ranking sections
            var rank = 1;
            foreach (var entryScore in tallyResult.FinalRanking)
            {
                var rankSection = new EntryTrophySection
                {
                    Entry = entryScore.Entry,
                    Rank = rank,
                    Suffix = "rank",
                    XOffset = 0,
                    YOffset = (rank - 1) * 140,
                };
                rankSections.Add(rankSection.Entry, rankSection);
                yield return rankSection;
                rank++;
            }

            // generate awards sections
            var awardedEntries = new HashSet<JamEntry>();
            var awardIdx = 0;
            foreach (var award in tallyResult.AwardRankings)
            {
                var winnerIdx = 0;
                foreach (var winner in award.GetWinners())
                {
                    var matchingSection = rankSections[winner.Entry];
                    awardedEntries.Add(winner.Entry);
                    yield return new EntryTrophySection
                    {
                        Entry = matchingSection.Entry,
                        Rank = matchingSection.Rank,
                        Suffix = award.Award.Id,
                        XOffset = 600 + winnerIdx * 480,
                        YOffset = awardIdx * 140,
                    };
                    winnerIdx++;
                }
                awardIdx++;
            }

            // generate combined sections
            foreach (var entry in awardedEntries)
            {
                var matchingSection = rankSections[entry];
                yield return new EntryTrophySection
                {
                    Entry = entry,
                    Rank = matchingSection.Rank,
                    Suffix = "all",
                    XOffset = -600,
                    YOffset = matchingSection.YOffset,
                };
            }
        }
    }
}
