using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Files
{
    public class JamAfterwordEditable
    {
        public JamFilesEditable Files { get; }
        public string? Location { get; set; }

        public FilePath FullLocation => Files.DirectoryPath.Append(Location ?? string.Empty);
        public bool CanOpen => FullLocation.HasFile();

        public JamAfterwordEditable(JamFilesEditable files)
        {
            Files = files;
        }
    }
}
