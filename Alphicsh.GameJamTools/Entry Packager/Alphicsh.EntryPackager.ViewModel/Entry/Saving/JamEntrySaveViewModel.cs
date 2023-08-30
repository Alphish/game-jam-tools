using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.EntryPackager.Model.Entry.Saving;
using Alphicsh.JamTools.Common.Mvvm.Saving;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Saving
{
    public class JamEntrySaveViewModel : SaveViewModel<JamEntryEditable, JamEntryEditableViewModel>
    {
        public JamEntrySaveViewModel() : base(
            new JamEntrySaveModel(isFromEntryPackager: true),
            new JamEntrySaveDataObserver()
            )
        {
        }
    }
}
