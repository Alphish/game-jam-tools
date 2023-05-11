namespace Alphicsh.JamTools.Common.IO.Execution
{
    public class FilesystemLauncher
    {
        private ProcessLauncher InnerLauncher { get; } = new ProcessLauncher();

        public static bool CanLaunchFrom(FilePath path)
        {
            return path.HasFile();
        }

        public void LaunchFrom(FilePath path)
        {
            if (!CanLaunchFrom(path))
                return;

            InnerLauncher.OpenFile(path);
        }

        public static bool CanOpenDirectoryFrom(FilePath path)
        {
            return path.HasDirectory();
        }

        public void OpenDirectoryFrom(FilePath path)
        {
            if (!CanOpenDirectoryFrom(path))
                return;

            InnerLauncher.OpenDirectory(path);
        }
    }
}
