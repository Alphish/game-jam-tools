using System;

namespace Alphicsh.JamTools.Common.IO.Execution
{
    public class LaunchData
    {
        public string Name { get; }
        public string? Description { get; }
        public LaunchType Type { get; }
        public string Location { get; }

        public LaunchData(string name, string? description, LaunchType type, string location)
        {
            Name = name;
            Description = description;
            Location = location;
            Type = type;
        }

        public bool CanLaunch()
        {
            switch (Type)
            {
                case LaunchType.WindowsExe:
                    var path = FilePath.From(Location);
                    return FilesystemLauncher.CanLaunchFrom(path);
                case LaunchType.WebLink:
                    var websiteUri = MakeUri();
                    return websiteUri != null && WebsiteLauncher.CanLaunchFrom(websiteUri);
                case LaunchType.GxGamesLink:
                    var gxGameUri = MakeUri();
                    return gxGameUri != null && GxGameLauncher.CanLaunchFrom(gxGameUri);
                default:
                    return false;
            }
        }

        private Uri? MakeUri()
        {
            return Uri.TryCreate(Location, UriKind.Absolute, out var uri) ? uri : null;
        }
    }
}
