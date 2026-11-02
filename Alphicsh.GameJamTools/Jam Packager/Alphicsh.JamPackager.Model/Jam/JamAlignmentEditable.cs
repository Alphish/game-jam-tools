using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPackager.Model.Jam
{
    public class JamAlignmentEditable
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string ShortTitle { get; set; } = default!;

        public IReadOnlyList<string> Keywords { get; set; } = new List<string>();
        public string? KeywordsString
        {
            get => Keywords.Count != 0 ? string.Join(", ", Keywords) : null;
            set => Keywords = value != null ? value.Split(',').Select(str => str.Trim()).ToList() : new List<string>();
        }

        public string? Color { get; set; }
    }
}
