using System;
using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Execution
{
    public class GxGameLauncher
    {
        private ProcessLauncher InnerLauncher { get; } = new ProcessLauncher();

        private static HashSet<string> ValidGxGamesSchemes { get; }
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "https" };

        private static HashSet<string> ValidGxGamesAuthorities { get; }
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "gx.games" };

        public static bool CanLaunchFrom(Uri gxGameUri)
        {
            if (!ValidGxGamesSchemes.Contains(gxGameUri.Scheme))
                return false;

            if (!ValidGxGamesAuthorities.Contains(gxGameUri.Authority))
                return false;

            return true;
        }

        public void LaunchFrom(Uri gxGameUri)
        {
            if (!CanLaunchFrom(gxGameUri))
                return;

            // TODO: Make GX.games location configurable
            InnerLauncher.OpenGxGame(@"%LOCALAPPDATA%\Programs\Opera GX\opera.exe", gxGameUri);
        }
    }
}
