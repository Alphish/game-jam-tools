using System.IO;
using System.Windows.Input;
using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.JamTools.Common.Controls.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files
{
    public class JamReadmeEditableViewModel : WrapperViewModel<JamReadmeEditable>
    {
        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();

        public JamReadmeEditableViewModel(JamReadmeEditable model) : base(model)
        {
            LocationProperty = WrapperProperty.ForMember(this, vm => vm.Model.Location);
            IsRequiredProperty = WrapperProperty.ForMember(this, vm => vm.Model.IsRequired);

            SearchCommand = SimpleCommand.From(SearchReadme);
            OpenReadmeCommand = ConditionalCommand.From(CanOpenReadme, OpenReadme)
                .ExecutionDependingOn(LocationProperty);
        }

        public WrapperProperty<JamReadmeEditableViewModel, string?> LocationProperty { get; }
        public string? Location { get => LocationProperty.Value; set => LocationProperty.Value = value; }

        public WrapperProperty<JamReadmeEditableViewModel, bool> IsRequiredProperty { get; }
        public bool IsRequired { get => IsRequiredProperty.Value; set => IsRequiredProperty.Value = value; }

        // ------
        // Search
        // ------

        public ICommand SearchCommand { get; }
        private void SearchReadme()
        {
            var filesPath = Model.Files.DirectoryPath;
            var path = FileQuery.OpenFile()
                .WithFileType("*.txt", "Text files")
                .WithFileType("*.md", "Markdown files")
                .WithFileType("*.rtf", "Rich Text Format")
                .WithFileType("*.pdf", "Portable Document Format")
                .WithFileType("*.*", "All files")
                .FromDirectory(Model.Files.DirectoryPath)
                .GetPath();

            if (path == null)
                return;

            var foundPath = path.Value;
            if (!foundPath.Value.StartsWith(filesPath.Value))
            {
                var newPath = filesPath.Append("README" + foundPath.GetExtension());
                File.Copy(foundPath.Value, newPath.Value, overwrite: true);
                foundPath = newPath;
            }

            var subpath = foundPath.AsRelativeTo(filesPath);
            Location = subpath.Value;
        }

        // -------
        // Opening
        // -------

        public IConditionalCommand OpenReadmeCommand { get; }
        private bool CanOpenReadme() => Model.CanOpen;
        private void OpenReadme() => ProcessLauncher.OpenFile(Model.FullLocation!.Value);
    }
}
