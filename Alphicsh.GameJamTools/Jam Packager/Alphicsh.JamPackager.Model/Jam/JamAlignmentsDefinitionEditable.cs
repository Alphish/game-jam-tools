using System.Collections.Generic;

namespace Alphicsh.JamPackager.Model.Jam
{
    public class JamAlignmentsDefinitionEditable
    {
        public string NeitherTitle { get; set; } = default!;
        public IList<JamAlignmentEditable> Alignments { get; set; } = new List<JamAlignmentEditable>();

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(NeitherTitle) && Alignments.Count == 0;
        }
    }
}
