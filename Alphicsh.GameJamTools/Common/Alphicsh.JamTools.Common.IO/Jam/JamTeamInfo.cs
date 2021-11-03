using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamTeamInfo
    {
        public string? Name { get; init; }
        public IReadOnlyCollection<JamAuthorInfo> Authors { get; init; } = default!;
    }
}
