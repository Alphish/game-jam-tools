using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class JamEntryEditableViewModel : WrapperViewModel<JamEntryEditable>
    {
        public JamEntryEditableViewModel(JamEntryEditable model) : base(model)
        {
            TitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.Title);
        }

        public WrapperProperty<JamEntryEditableViewModel, string> TitleProperty { get; }
        public string Title { get => TitleProperty.Value; set => TitleProperty.Value = value; }
    }
}
