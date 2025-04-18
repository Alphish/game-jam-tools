using System.Windows.Input;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.EntryPackager.ViewModel.Entry.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class JamEntryEditableViewModel : WrapperViewModel<JamEntryEditable>
    {
        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();

        public JamEntryEditableViewModel(JamEntryEditable model) : base(model)
        {
            TitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.Title);
            ShortTitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.ShortTitle);
            AlignmentProperty = WrapperProperty.ForMember(this, vm => vm.Model.Alignment);
            DisplayShortTitleProperty = NotifiableProperty.Create(this, nameof(DisplayShortTitle))
                .DependingOn(TitleProperty, ShortTitleProperty);

            Team = new JamTeamEditableViewModel(model.Team);

            Files = new JamFilesEditableViewModel(model.Files);

            HasLauncherProperty = NotifiableProperty.Create(this, nameof(HasLauncher))
                .DependingOn(Files.Launchers.HasLaunchersProperty);
            HasRequiredReadmeProperty = NotifiableProperty.Create(this, nameof(HasRequiredReadme))
                .DependingOn(Files.Readme.LocationProperty, Files.Readme.IsRequiredProperty);
            HasRegularReadmeProperty = NotifiableProperty.Create(this, nameof(HasRegularReadme))
                .DependingOn(Files.Readme.LocationProperty, Files.Readme.IsRequiredProperty);
            HasAfterwordProperty = NotifiableProperty.Create(this, nameof(HasAfterword))
                .DependingOn(Files.Afterword.LocationProperty);

            OpenDirectoryCommand = SimpleCommand.From(OpenDirectory);
        }

        // -----------------
        // Basic information
        // -----------------

        public WrapperProperty<JamEntryEditableViewModel, string> TitleProperty { get; }
        public string Title { get => TitleProperty.Value; set => TitleProperty.Value = value; }

        public WrapperProperty<JamEntryEditableViewModel, string?> ShortTitleProperty { get; }
        public string? ShortTitle { get => ShortTitleProperty.Value; set => ShortTitleProperty.Value = value; }

        public WrapperProperty<JamEntryEditableViewModel, string?> AlignmentProperty { get; }
        public string? Alignment { get => AlignmentProperty.Value; set => AlignmentProperty.Value = value; }

        public NotifiableProperty DisplayShortTitleProperty { get; }
        public string DisplayShortTitle => Model.DisplayShortTitle;

        public JamTeamEditableViewModel Team { get; }

        // -----
        // Files
        // -----

        public JamFilesEditableViewModel Files { get; }

        // Readme & Afterword

        public NotifiableProperty HasLauncherProperty { get; }
        public bool HasLauncher => Files.Launchers.HasLaunchers;
        public NotifiableProperty HasRequiredReadmeProperty { get; }
        public bool HasRequiredReadme => Files.Readme.Model.CanOpen && Files.Readme.IsRequired;
        public NotifiableProperty HasRegularReadmeProperty { get; }
        public bool HasRegularReadme => Files.Readme.Model.CanOpen && !Files.Readme.IsRequired;
        public ICommand OpenReadmeCommand => Files.Readme.OpenReadmeCommand;

        public NotifiableProperty HasAfterwordProperty { get; }
        public bool HasAfterword => Files.Afterword.Model.CanOpen;
        public ICommand OpenAfterwordCommand => Files.Afterword.OpenAfterwordCommand;

        // Directory

        public ICommand OpenDirectoryCommand { get; }
        private void OpenDirectory()
        {
            ProcessLauncher.OpenDirectory(Files.Model.DirectoryPath);
        }
    }
}
