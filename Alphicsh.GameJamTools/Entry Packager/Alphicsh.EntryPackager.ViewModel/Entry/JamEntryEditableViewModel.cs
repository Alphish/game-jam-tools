using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.EntryPackager.ViewModel.Entry.Files;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class JamEntryEditableViewModel : WrapperViewModel<JamEntryEditable>
    {
        public JamEntryEditableViewModel(JamEntryEditable model) : base(model)
        {
            TitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.Title);
            ShortTitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.ShortTitle);

            Team = new JamTeamEditableViewModel(model.Team);
            Files = new JamFilesEditableViewModel(model.Files);
        }

        public WrapperProperty<JamEntryEditableViewModel, string> TitleProperty { get; }
        public string Title { get => TitleProperty.Value; set => TitleProperty.Value = value; }

        public WrapperProperty<JamEntryEditableViewModel, string?> ShortTitleProperty { get; }
        public string? ShortTitle { get => ShortTitleProperty.Value; set => ShortTitleProperty.Value = value; }

        public JamTeamEditableViewModel Team { get; }

        public JamFilesEditableViewModel Files { get; }
    }
}
