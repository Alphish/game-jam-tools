using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Jam
{
    public class JamAlignmentInfo
    {
        public string NeitherTitle { get; init; } = default!;
        public IReadOnlyCollection<JamAlignmentOptionInfo> Options { get; init; } = default!;
    }
}
