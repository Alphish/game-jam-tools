using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.EntryPackager.Model.Entry.Saving
{
    public class JamEntrySaveModel : SaveModel<JamEntryEditable, JamEntrySaveData>, ISaveModel<JamEntryEditable>
    {
        public JamEntrySaveModel() : base(
            new JamEntrySaveDataExtractor(),
            new JamEntryDataSaver()
            )
        {
        }
    }
}
