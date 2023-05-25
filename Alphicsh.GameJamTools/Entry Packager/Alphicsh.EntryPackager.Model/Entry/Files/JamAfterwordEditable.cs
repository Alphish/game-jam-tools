using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Files
{
    public class JamAfterwordEditable
    {
        public JamFilesEditable Files { get; }
        public string? Location { get; set; }

        public FilePath? FullLocation => !string.IsNullOrEmpty(Location) ? Files.DirectoryPath.Append(Location) : null;
        public bool CanOpen => FullLocation.HasValue && FullLocation.Value.HasFile();
        public bool IsEmpty => string.IsNullOrWhiteSpace(Location);

        public JamAfterwordEditable(JamFilesEditable files)
        {
            Files = files;
        }

        public void ClearInvalid()
        {
            if (!CanOpen)
                Location = null;
        }
    }
}
