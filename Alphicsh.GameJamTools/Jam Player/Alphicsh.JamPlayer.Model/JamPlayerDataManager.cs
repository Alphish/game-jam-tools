using Alphicsh.JamTools.Common.IO;

namespace Alphicsh.JamPlayer.Model
{
    public class JamPlayerDataManager
    {
        public AppModel AppModel { get; init; } = default!;
        public FilePath DirectoryPath => AppModel.Jam.DirectoryPath.Append(".jamplayer");
    }
}
