using Alphicsh.JamTools.Common.IO.Jam.Entries;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class ComponentAttributionEntry
    {
        public string Description { get; }
        public ComponentAttribution Value { get; }

        public ComponentAttributionEntry(string description, ComponentAttribution value)
        {
            Description = description;
            Value = value;
        }
    }
}
