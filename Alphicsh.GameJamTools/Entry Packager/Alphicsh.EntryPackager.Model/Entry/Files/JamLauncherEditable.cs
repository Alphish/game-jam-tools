using Alphicsh.JamTools.Common.IO.Execution;

namespace Alphicsh.EntryPackager.Model.Entry.Files
{
    public class JamLauncherEditable
    {
        public JamFilesEditable Files { get; }

        public LaunchData LaunchData { get; private set; } = default!;
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public LaunchType Type { get; private set; }
        public string Location { get; private set; }

        public bool CanLaunch => LaunchData.CanLaunch();

        public JamLauncherEditable(JamFilesEditable files)
        {
            Files = files;
            Name = "Exe file";
            Description = null;
            Type = LaunchType.WindowsExe;
            Location = "";
            UpdateLaunchData();
        }

        public void UpdateLaunchData()
        {
            var shouldPrependDirectory = Type == LaunchType.WindowsExe && Files.DirectoryPath.HasDirectory();
            var fullLocation = shouldPrependDirectory ? Files.DirectoryPath.Append(Location).Value : Location;
            LaunchData = new LaunchData(Name, Description, Type, fullLocation);
        }

        public void SetName(string value)
        {
            Name = value;
            UpdateLaunchData();
        }

        public void SetDescription(string? value)
        {
            Description = value;
            UpdateLaunchData();
        }

        public void SetType(LaunchType value)
        {
            Type = value;
            UpdateLaunchData();
        }

        public void SetLocation(string value)
        {
            Location = value;
            UpdateLaunchData();
        }
    }
}
