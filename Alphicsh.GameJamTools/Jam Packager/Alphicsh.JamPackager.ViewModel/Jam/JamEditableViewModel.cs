using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Alphicsh.EntryPackager.ViewModel.Entry;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPackager.ViewModel.Jam
{
    public class JamEditableViewModel : WrapperViewModel<JamEditable>
    {
        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();

        public JamEditableViewModel(JamEditable model) : base(model)
        {
            OpenDirectoryCommand = SimpleCommand.From(OpenDirectory);
            SelectEntriesSubpathCommand = SimpleCommand.From(SelectEntriesSubpath);

            LogoLocationProperty = NotifiableProperty.Create(this, nameof(LogoLocation));
            LogoProperty = ImageSourceProperty
                .CreateReadonly(this, nameof(Logo), vm => vm.Model.LogoPath)
                .DependingOn(LogoLocationProperty);
            LogoWidthProperty = NotifiableProperty.Create(this, nameof(LogoWidth))
                .DependingOn(LogoProperty);
            LogoHeightProperty = NotifiableProperty.Create(this, nameof(LogoHeight))
                .DependingOn(LogoProperty);

            SearchLogoCommand = SimpleCommand.From(SearchLogo);

            TitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.Title);
            ThemeProperty = WrapperProperty.ForMember(this, vm => vm.Model.Theme);

            StartTimeProperty = WrapperProperty.ForMember(this, vm => vm.Model.StartTime);
            EndTimeProperty = WrapperProperty.ForMember(this, vm => vm.Model.EndTime);
            HostsProperty = WrapperProperty.Create(this, nameof(Hosts), vm => vm.Model.GetHostsString(), (vm, value) => vm.Model.SetHostsString(value));

            Links = CollectionViewModel.CreateMutable(Model.Links, JamLinkEditableViewModel.CollectionStub);
            AddLinkCommand = SimpleCommand.From(AddLink);
            RemoveLinkCommand = SimpleCommand.WithParameter<JamLinkEditableViewModel>(RemoveLink);
            OpenLinkCommand = SimpleCommand.WithParameter<JamLinkEditableViewModel>(OpenLink);

            Awards = CollectionViewModel.CreateMutable(Model.Awards, JamAwardEditableViewModel.CollectionStub);
            AddAwardCommand = SimpleCommand.From(AddAward);
            RemoveAwardCommand = SimpleCommand.WithParameter<JamAwardEditableViewModel>(RemoveAward);

            Entries = Model.Entries.Select(model => new JamEntryEditableViewModel(model)).ToList();
            SelectedEntryProperty = MutableProperty.Create(this, nameof(SelectedEntry), initialValue: (JamEntryEditableViewModel?)null);
        }

        // -----------
        // Directories
        // -----------

        public string DirectoryPath => Model.DirectoryPath.Value;

        public ICommand OpenDirectoryCommand { get; }
        private void OpenDirectory()
        {
            ProcessLauncher.OpenDirectory(Model.DirectoryPath);
        }

        public string EntriesLocation => Model.EntriesLocation;
        public ICommand SelectEntriesSubpathCommand { get; }
        private void SelectEntriesSubpath()
        {
            var entriesPath = FileQuery.OpenDirectory().GetPath();
            if (entriesPath == null)
                return;

            if (!entriesPath.Value.IsSubpathOf(Model.DirectoryPath))
            {
                SimpleMessageViewModel.ShowModal("Invalid path", "The chosen entries directory must be inside the jam directory.");
                return;
            }

            Model.SetEntriesPath(entriesPath.Value);
            Entries = Model.Entries.Select(model => new JamEntryEditableViewModel(model)).ToList();
            RaisePropertyChanged(nameof(EntriesLocation), nameof(Entries));
        }

        // ----
        // Logo
        // ----

        public ImageSourceProperty<JamEditableViewModel> LogoProperty { get; }
        public BitmapSource? Logo => LogoProperty.ImageSource as BitmapSource;

        public NotifiableProperty LogoWidthProperty { get; }
        public double LogoWidth => Logo?.PixelWidth ?? 120d;

        public NotifiableProperty LogoHeightProperty { get; }
        public double LogoHeight => Logo?.PixelHeight ?? 120d;

        public NotifiableProperty LogoLocationProperty { get; }
        public string? LogoLocation => Model.LogoLocation;
        public ICommand SearchLogoCommand { get; }
        private void SearchLogo()
        {
            var logoPath = FileQuery.OpenFile()
                .FromDirectory(Model.DirectoryPath)
                .WithFileType("*.png;*.jpg;*.jpeg;*.bmp", "Image files")
                .GetPath();

            if (logoPath == null)
                return;

            var foundPath = logoPath.Value;
            if (!foundPath.IsSubpathOf(Model.DirectoryPath))
            {
                var newPath = Model.DirectoryPath.Append("logo" + foundPath.GetExtension());
                File.Copy(foundPath.Value, newPath.Value, overwrite: true);
                foundPath = newPath;
            }

            Model.LogoLocation = foundPath.AsRelativeTo(Model.DirectoryPath).Value;
            LogoLocationProperty.RaisePropertyChanged();
        }

        // -----------
        // Information
        // -----------

        public WrapperProperty<JamEditableViewModel, string?> TitleProperty { get; }
        public string? Title { get => TitleProperty.Value; set => TitleProperty.Value = value; }

        public WrapperProperty<JamEditableViewModel, string?> ThemeProperty { get; }
        public string? Theme { get => ThemeProperty.Value; set => ThemeProperty.Value = value; }

        public WrapperProperty<JamEditableViewModel, string?> StartTimeProperty { get; }
        public string? StartTime { get => StartTimeProperty.Value; set => StartTimeProperty.Value = value; }

        public WrapperProperty<JamEditableViewModel, string?> EndTimeProperty { get; }
        public string? EndTime { get => EndTimeProperty.Value; set => EndTimeProperty.Value = value; }

        public WrapperProperty<JamEditableViewModel, string?> HostsProperty { get; }
        public string? Hosts { get => HostsProperty.Value; set => HostsProperty.Value = value; }

        // -----
        // Links
        // -----

        public CollectionViewModel<JamLinkEditable, JamLinkEditableViewModel> Links { get; }

        public ICommand AddLinkCommand { get; }
        private void AddLink()
        {
            var linkModel = new JamLinkEditable { Title = "Example", Url = "http://example.com/" };
            var linkVm = new JamLinkEditableViewModel(linkModel);
            Links.Add(linkVm);
            Links.CompleteChanges();
        }

        public ICommand RemoveLinkCommand { get; }
        private void RemoveLink(JamLinkEditableViewModel linkVm)
        {
            Links.Remove(linkVm);
            Links.CompleteChanges();
        }

        private static WebsiteLauncher LinkLauncher { get; } = new WebsiteLauncher();

        public ICommand OpenLinkCommand { get; }
        private void OpenLink(JamLinkEditableViewModel linkVm)
        {
            if (!Uri.TryCreate(linkVm.Url, UriKind.Absolute, out var uri))
                return;

            LinkLauncher.LaunchFrom(uri);
        }

        // ------
        // Awards
        // ------

        public CollectionViewModel<JamAwardEditable, JamAwardEditableViewModel> Awards { get; }

        public ICommand AddAwardCommand { get; }
        private void AddAward()
        {
            var awardModel = new JamAwardEditable { Id = "award", Name = "Best of Awards" };
            var awardVm = new JamAwardEditableViewModel(awardModel);
            Awards.Add(awardVm);
            Awards.CompleteChanges();
        }

        public ICommand RemoveAwardCommand { get; }
        private void RemoveAward(JamAwardEditableViewModel awardVm)
        {
            Awards.Remove(awardVm);
            Awards.CompleteChanges();
        }

        // -------
        // Entries
        // -------

        public IReadOnlyCollection<JamEntryEditableViewModel> Entries { get; private set; }

        public MutableProperty<JamEntryEditableViewModel?> SelectedEntryProperty { get; }
        public JamEntryEditableViewModel? SelectedEntry { get => SelectedEntryProperty.Value; set => SelectedEntryProperty.Value = value; }
    }
}
