using System.ComponentModel;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class JamEntryEditableViewModel : WrapperViewModel<JamEntryEditable>
    {
        public JamEntryEditableViewModel(JamEntryEditable model) : base(model)
        {
            DirectoryNameProperty = WrapperProperty.ForMember(this, vm => vm.Model.DirectoryName);
            DefaultDirectoryNameProperty = WrapperProperty
                .ForReadonlyMember(this, nameof(DefaultDirectoryName), vm => vm.Model.GetDefaultDirectoryName());
            TitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.Title)
                .WithDependingProperty(nameof(DefaultDirectoryName));

            Team = new JamTeamEditableViewModel(model.Team);
            Team.PropertyChanged += Team_PropertyChanged;
        }

        public WrapperProperty<JamEntryEditableViewModel, string?> DirectoryNameProperty { get; }
        public string? DirectoryName { get => DirectoryNameProperty.Value; set => DirectoryNameProperty.Value = value; }

        public WrapperProperty<JamEntryEditableViewModel, string> DefaultDirectoryNameProperty { get; }
        public string DefaultDirectoryName { get => DefaultDirectoryNameProperty.Value; }

        public WrapperProperty<JamEntryEditableViewModel, string> TitleProperty { get; }
        public string Title { get => TitleProperty.Value; set => TitleProperty.Value = value; }

        public JamTeamEditableViewModel Team { get; }

        private void Team_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Team.Name) && e.PropertyName != nameof(Team.AuthorsString))
                return;

            RaisePropertyChanged(nameof(DefaultDirectoryName));
        }
    }
}
