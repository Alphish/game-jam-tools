using System.Linq;

using Alphicsh.JamTools.Common.IO.Jam;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public class JamInfoMapper
    {
        public JamOverview MapInfoToJam(JamInfo info)
        {
            return new JamOverview
            {
                Entries = info.Entries.Select(MapInfoToEntry).ToList(),
            };
        }

        private JamEntry MapInfoToEntry(JamEntryInfo info)
        {
            return new JamEntry
            {
                Title = info.Title,
                Team = MapInfoToTeam(info.Team),
                ThumbnailPath = info.ThumbnailPath,
                ThumbnailSmallPath = info.ThumbnailSmallPath,
            };
        }

        private JamTeam MapInfoToTeam(JamTeamInfo info)
        {
            return new JamTeam
            {
                Name = info.Name,
                Authors = info.Authors.Select(MapInfoToAuthor).ToList(),
            };
        }

        private JamAuthor MapInfoToAuthor(JamAuthorInfo info)
        {
            return new JamAuthor
            {
                Name = info.Name,
            };
        }
    }
}
