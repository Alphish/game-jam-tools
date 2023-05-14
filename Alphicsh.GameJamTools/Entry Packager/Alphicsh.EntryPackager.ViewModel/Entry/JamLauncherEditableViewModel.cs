using System.Windows;
using System.Windows.Input;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamTools.Common.Controls;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class JamLauncherEditableViewModel : WrapperViewModel<JamLauncherEditable>
    {
        public static CollectionViewModelStub<JamLauncherEditable, JamLauncherEditableViewModel> CollectionStub
            => CollectionViewModelStub.Create((JamLauncherEditable model) => new JamLauncherEditableViewModel(model));

        private static EntryLauncher EntryLauncher { get; } = new EntryLauncher();

        public JamLauncherEditableViewModel(JamLauncherEditable model) : base(model)
        {
            LaunchCommand = ConditionalCommand.From(CanExecuteLaunch, ExecuteLaunch);
            SearchExecutableCommand = SimpleCommand.From(SearchExecutable);

            LocationBarPositionProperty = NotifiableProperty.Create(this, nameof(LocationBarPosition));
            SearchVisibilityProperty = NotifiableProperty.Create(this, nameof(SearchVisibility));

            NameProperty = WrapperProperty
                .Create(this, nameof(Name), vm => vm.Model.Name, (vm, value) => vm.Model.SetName(value));
            DescriptionProperty = WrapperProperty
                .Create(this, nameof(Description), vm => vm.Model.Description, (vm, value) => vm.Model.SetDescription(value));
            TypeProperty = WrapperProperty
                .Create(this, nameof(Type), vm => vm.Model.Type, (vm, value) => vm.Model.SetType(value))
                .WithDependingCommand(LaunchCommand)
                .WithDependingProperty(LocationBarPositionProperty)
                .WithDependingProperty(SearchVisibilityProperty);
            LocationProperty = WrapperProperty
                .Create(this, nameof(Location), vm => vm.Model.Location, (vm, value) => vm.Model.SetLocation(value))
                .WithDependingCommand(LaunchCommand);
        }

        // ----------------
        // Basic properties
        // ----------------

        public WrapperProperty<JamLauncherEditableViewModel, string> NameProperty { get; }
        public string Name { get => NameProperty.Value; set => NameProperty.Value = value; }

        public WrapperProperty<JamLauncherEditableViewModel, string?> DescriptionProperty { get; }
        public string? Description { get => DescriptionProperty.Value; set => DescriptionProperty.Value = value; }

        public WrapperProperty<JamLauncherEditableViewModel, LaunchType> TypeProperty { get; }
        public LaunchType Type { get => TypeProperty.Value; set => TypeProperty.Value = value; }

        public WrapperProperty<JamLauncherEditableViewModel, string> LocationProperty { get; }
        public string Location { get => LocationProperty.Value; set => LocationProperty.Value = value; }

        // ---------------
        // Search handling
        // ---------------

        public NotifiableProperty LocationBarPositionProperty { get; }
        public BarPosition LocationBarPosition { get => Type == LaunchType.WindowsExe ? BarPosition.Start : BarPosition.Fill; }

        public NotifiableProperty SearchVisibilityProperty { get; }
        public Visibility SearchVisibility { get => Type == LaunchType.WindowsExe ? Visibility.Visible : Visibility.Collapsed; }

        public ICommand SearchExecutableCommand { get; }
        private void SearchExecutable()
        {
            var filesPath = Model.Files.DirectoryPath;
            var path = FileQuery.OpenFile()
                .WithFileType("*.exe", "Windows executable")
                .FromDirectory(Model.Files.DirectoryPath)
                .GetPath();

            if (path == null)
                return;

            if (!path.Value.Value.StartsWith(filesPath.Value))
            {
                SimpleMessageViewModel.ShowModal("Invalid path", "The chosen executable must be inside the entry directory.");
                return;
            }

            var subpath = path.Value.AsRelativeTo(filesPath);
            Location = subpath.Value;
        }

        // ---------
        // Launching
        // ---------

        public IConditionalCommand LaunchCommand { get; }
        private bool CanExecuteLaunch()
        {
            return Model.LaunchData.CanLaunch();
        }
        private void ExecuteLaunch()
        {
            EntryLauncher.Launch(Model.LaunchData);
        }
    }
}
