using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.EntryPackager.Model.Entry.Files
{
    public class JamReadmeEditable
    {
        public JamFilesEditable Files { get; }
        public string? Location { get; set; }
        public bool IsRequired { get; set; }

        public FilePath? FullLocation => Location != null ? Files.DirectoryPath.Append(Location) : null;
        public bool CanOpen => FullLocation.HasValue && FullLocation.Value.HasFile();

        public JamReadmeEditable(JamFilesEditable files)
        {
            Files = files;
        }

        public void ClearInvalid()
        {
            if (!CanOpen)
            {
                Location = null;
                IsRequired = false;
            }
        }
    }
}
