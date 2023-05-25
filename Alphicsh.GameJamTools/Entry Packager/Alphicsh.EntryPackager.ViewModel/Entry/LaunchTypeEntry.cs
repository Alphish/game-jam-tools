using Alphicsh.JamTools.Common.IO.Execution;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class LaunchTypeEntry
    {
        public string Description { get; }
        public LaunchType Value { get; }

        public LaunchTypeEntry(string description, LaunchType value)
        {
            Description = description;
            Value = value;
        }
    }
}
