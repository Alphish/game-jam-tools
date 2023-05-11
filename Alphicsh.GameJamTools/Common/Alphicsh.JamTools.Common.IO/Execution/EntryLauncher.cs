using System;

namespace Alphicsh.JamTools.Common.IO.Execution
{
    public class EntryLauncher
    {
        private FilesystemLauncher FilesystemLauncher { get; } = new FilesystemLauncher();
        private WebsiteLauncher WebsiteLauncher { get; } = new WebsiteLauncher();
        private GxGameLauncher GxGameLauncher { get; } = new GxGameLauncher();

        public void Launch(LaunchData launchData)
        {
            switch (launchData.Type)
            {
                case LaunchType.WindowsExe:
                    var path = FilePath.From(launchData.Location);
                    FilesystemLauncher.LaunchFrom(path);
                    break;
                case LaunchType.WebLink:
                    var websiteUri = new Uri(launchData.Location);
                    WebsiteLauncher.LaunchFrom(websiteUri);
                    break;
                case LaunchType.GxGamesLink:
                    var gxGameUri = new Uri(launchData.Location);
                    GxGameLauncher.LaunchFrom(gxGameUri);
                    break;
                default:
                    return;
            }
        }
    }
}
