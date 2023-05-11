using System;
using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Execution
{
    public class WebsiteLauncher
    {
        private ProcessLauncher InnerLauncher { get; } = new ProcessLauncher();

        private static HashSet<string> ValidWebsiteSchemes { get; }
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "http", "https" };

        public static bool CanLaunchFrom(Uri websiteUri)
        {
            return ValidWebsiteSchemes.Contains(websiteUri.Scheme);
        }

        public void LaunchFrom(Uri websiteUri)
        {
            if (!CanLaunchFrom(websiteUri))
                return;

            InnerLauncher.OpenWebsite(websiteUri);
        }
    }
}
