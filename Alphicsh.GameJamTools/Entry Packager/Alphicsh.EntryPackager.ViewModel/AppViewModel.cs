using Alphicsh.EntryPackager.Model;
using Alphicsh.EntryPackager.ViewModel.Entry;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.EntryPackager.ViewModel
{
    public class AppViewModel : WrapperViewModel<AppModel>
    {
        public AppViewModel(AppModel model) : base(model)
        {
            Entry = new JamEntryEditableViewModel(model.Entry);
        }

        public JamEntryEditableViewModel Entry { get; }
    }
}
