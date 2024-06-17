using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Jam.New
{
    public class NewJamAlignmentInfo
    {
        public string NeitherTitle { get; init; } = default!;
        public IReadOnlyCollection<NewJamAlignmentOptionInfo> Options { get; init; } = default!;
    }
}
