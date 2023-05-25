using Alphicsh.EntryPackager.Model.Entry.Files;
using Alphicsh.JamTools.Common.IO.Execution;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Controls.Files;
using System.Windows.Input;
using System.IO;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Files
{
    public class JamAfterwordEditableViewModel : WrapperViewModel<JamAfterwordEditable>
    {
        private static ProcessLauncher ProcessLauncher { get; } = new ProcessLauncher();

        public JamAfterwordEditableViewModel(JamAfterwordEditable model) : base(model)
        {
            LocationProperty = WrapperProperty.ForMember(this, vm => vm.Model.Location);

            SearchCommand = SimpleCommand.From(SearchAfterword);
            OpenAfterwordCommand = ConditionalCommand.From(CanOpenAfterword, OpenAfterword)
                .ExecutionDependingOn(LocationProperty);
        }

        public WrapperProperty<JamAfterwordEditableViewModel, string?> LocationProperty { get; }
        public string? Location { get => LocationProperty.Value; set => LocationProperty.Value = value; }

        // ------
        // Search
        // ------

        public ICommand SearchCommand { get; }
        private void SearchAfterword()
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
                var newPath = filesPath.Append("AFTERWORD" + foundPath.GetExtension());
                File.Copy(foundPath.Value, newPath.Value, overwrite: true);
                foundPath = newPath;
            }

            var subpath = foundPath.AsRelativeTo(filesPath);
            Location = subpath.Value;
        }

        // -------
        // Opening
        // -------

        public IConditionalCommand OpenAfterwordCommand { get; }
        private bool CanOpenAfterword() => Model.CanOpen;
        private void OpenAfterword() => ProcessLauncher.OpenFile(Model.FullLocation!.Value);
    }
}
