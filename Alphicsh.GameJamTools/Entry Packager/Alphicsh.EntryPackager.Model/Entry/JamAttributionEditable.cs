using Alphicsh.JamTools.Common.IO.Jam.Entries;

namespace Alphicsh.EntryPackager.Model.Entry
{
    public class JamAttributionEditable
    {
        public ComponentAttribution Code { get; set; } = ComponentAttribution.Unspecified;
        public ComponentAttribution Visuals { get; set; } = ComponentAttribution.Unspecified;
        public ComponentAttribution Music { get; set; } = ComponentAttribution.Unspecified;
        public ComponentAttribution Sounds { get; set; } = ComponentAttribution.Unspecified;
    }
}
