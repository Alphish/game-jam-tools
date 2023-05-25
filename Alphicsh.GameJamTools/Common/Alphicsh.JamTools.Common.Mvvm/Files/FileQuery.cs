using Alphicsh.JamTools.Common.Mvvm.Files;

namespace Alphicsh.JamTools.Common.Controls.Files
{
    public static class FileQuery
    {
        public static OpenFileQuery OpenFile()
        {
            return new OpenFileQuery();
        }

        public static OpenDirectoryQuery OpenDirectory()
        {
            return new OpenDirectoryQuery();
        }

        public static SaveFileQuery SaveFile()
        {
            return new SaveFileQuery();
        }
    }
}
