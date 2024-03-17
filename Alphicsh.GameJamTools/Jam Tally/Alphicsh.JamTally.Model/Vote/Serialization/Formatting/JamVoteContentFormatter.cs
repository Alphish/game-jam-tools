using System.Text;

namespace Alphicsh.JamTally.Model.Vote.Serialization.Formatting
{
    internal class JamVoteContentFormatter
    {
        public string Format(JamVoteContent voteContent)
        {
            var builder = new StringBuilder();
            builder.AppendLine(voteContent.Header);

            foreach (var section in voteContent.Sections)
            {
                builder.AppendLine(section.Header);
                foreach (var line in section.Lines)
                {
                    if (line.StartsWith("#"))
                        builder.AppendLine("\\" + line);
                    else
                        builder.AppendLine(line);
                }
                builder.AppendLine();
            }

            return builder.ToString()
                .Replace("\r\n", "\n")
                .Replace("\r", "\n");
        }
    }
}
