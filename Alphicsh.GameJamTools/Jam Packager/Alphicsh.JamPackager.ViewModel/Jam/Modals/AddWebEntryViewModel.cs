using System;
using System.Linq;
using System.Windows.Input;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPackager.ViewModel.Jam.Modals
{
    public class AddWebEntryViewModel : ModalViewModel
    {
        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();

        public AddWebEntryViewModel(JamEditableViewModel jam) : base("Add web entry")
        {
            Jam = jam;

            TitleProperty = MutableProperty.Create(this, nameof(Title), "Title");
            ShortTitleProperty = MutableProperty.Create(this, nameof(ShortTitle), (string?)null);
            TeamProperty = MutableProperty.Create(this, nameof(Team), (string?)null);
            AuthorsProperty = MutableProperty.Create(this, nameof(Authors), "Author");
            UrlProperty = MutableProperty.Create(this, nameof(Url), "https://example.com");
            OpenUrlCommand = SimpleCommand.From(OpenUrl);

            AddCommand = SimpleCommand.From(Add);
        }

        public static void ShowModal(JamEditableViewModel jam)
        {
            var viewModel = new AddWebEntryViewModel(jam);
            viewModel.ShowOwnModal();
        }

        private JamEditableViewModel Jam { get; }

        public MutableProperty<string> TitleProperty { get; }
        public string Title { get => TitleProperty.Value; set => TitleProperty.Value = value; }

        public MutableProperty<string?> ShortTitleProperty { get; }
        public string? ShortTitle { get => ShortTitleProperty.Value; set => ShortTitleProperty.Value = value; }

        public MutableProperty<string?> TeamProperty { get; }
        public string? Team { get => TeamProperty.Value; set => TeamProperty.Value = value; }

        public MutableProperty<string> AuthorsProperty { get; }
        public string Authors { get => AuthorsProperty.Value; set => AuthorsProperty.Value = value; }

        public MutableProperty<string> UrlProperty { get; }
        public string Url { get => UrlProperty.Value; set => UrlProperty.Value = value; }

        public ICommand OpenUrlCommand { get; }
        private void OpenUrl()
        {
            if (Url == null)
                return;

            if (!Uri.TryCreate(Url, UriKind.Absolute, out var pageUri))
                return;

            ProcessLauncher.OpenWebsite(pageUri);
        }

        public ICommand AddCommand { get; }
        private void Add()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Authors) || string.IsNullOrWhiteSpace(Url))
                return;

            var entry = Jam.Model.AddWebEntry(Title, ShortTitle, Team, Authors, Url);
            Jam.Entries.SynchronizeWithModels();
            Jam.SelectedEntry = Jam.Entries.FirstOrDefault(vm => vm.Model == entry);

            Window.Close();
        }
    }
}
