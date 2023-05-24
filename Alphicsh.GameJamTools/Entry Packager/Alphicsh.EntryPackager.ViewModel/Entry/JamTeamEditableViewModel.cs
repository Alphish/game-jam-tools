using System.Windows.Input;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class JamTeamEditableViewModel : WrapperViewModel<JamTeamEditable>
    {
        public JamTeamEditableViewModel(JamTeamEditable model) : base(model)
        {
            NameProperty = WrapperProperty.ForMember(this, vm => vm.Model.Name);

            Authors = CollectionViewModel.CreateMutable(model.Authors, JamAuthorEditableViewModel.CollectionStubFor(this));
            AuthorsStringProperty = WrapperProperty
                .Create(this, nameof(AuthorsString), vm => vm.Model.GetAuthorsString(), (vm, value) => vm.Model.SetAuthorsString(value))
                .WithDependingCollection(Authors);

            HasTeamNameProperty = NotifiableProperty.Create(this, nameof(HasTeamName))
                .DependingOn(NameProperty);
            DisplayNameProperty = NotifiableProperty.Create(this, nameof(DisplayName))
                .DependingOn(NameProperty, AuthorsStringProperty);

            AddAuthorCommand = SimpleCommand.From(AddAuthor);
            RemoveAuthorCommand = SimpleCommand.WithParameter<JamAuthorEditableViewModel>(RemoveAuthor);
        }

        public WrapperProperty<JamTeamEditableViewModel, string?> NameProperty { get; }
        public string? Name { get => NameProperty.Value; set => NameProperty.Value = value; }

        public CollectionViewModel<JamAuthorEditable, JamAuthorEditableViewModel> Authors { get; }

        public WrapperProperty<JamTeamEditableViewModel, string> AuthorsStringProperty { get; }
        public string AuthorsString { get => AuthorsStringProperty.Value; set => AuthorsStringProperty.Value = value; }

        public NotifiableProperty HasTeamNameProperty { get; }
        public bool HasTeamName => !string.IsNullOrWhiteSpace(Model.Name);

        public NotifiableProperty DisplayNameProperty { get; }
        public string DisplayName => Model.DisplayName;

        public ICommand AddAuthorCommand { get; }
        public void AddAuthor()
        {
            var author = new JamAuthorEditable { Name = "Author" };
            var authorVm = new JamAuthorEditableViewModel(author, this);
            Authors.Add(authorVm);
            Authors.CompleteChanges();
            AuthorsStringProperty.RaisePropertyChanged();
        }

        public ICommand RemoveAuthorCommand { get; }
        public void RemoveAuthor(JamAuthorEditableViewModel authorVm)
        {
            Authors.Remove(authorVm);
            Authors.CompleteChanges();
            AuthorsStringProperty.RaisePropertyChanged();
        }
    }
}
