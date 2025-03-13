using System.Collections.Generic;
using System.Xml.Linq;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result.Trophies
{
    public class EntryTrophySection
    {
        public JamEntry Entry { get; init; } = default!;
        public int Rank { get; init; }
        public string Suffix { get; init; } = default!;
        public string ElementId => $"entry{Rank}_{Suffix}";

        public int XOffset { get; init; }
        public int YOffset { get; init; }

        public XElement CreateExportArea()
        {
            return InkElements.CreateExportArea(ElementId + "_export", "242424", XOffset, YOffset, 520, 120, Entry.Id + ".png");
        }

        public IEnumerable<XElement> CreateBoxes()
        {
            yield return InkElements.CreateBox("0080ff", XOffset, YOffset, 20, 120);
            yield return InkElements.CreateBox("ffffff", XOffset + 20, YOffset, 120, 120);

            yield return InkElements.CreateBox("0080ff", XOffset + 150, YOffset + 4, 370, 24);
            yield return InkElements.CreateBox("0080ff", XOffset + 150, YOffset + 32, 370, 56);
            yield return InkElements.CreateBox("0080ff", XOffset + 150, YOffset + 92, 370, 24);
        }
    }
}
