using System.Collections.Generic;
using System.Text;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Result
{
    internal class ResultsPostGenerator
    {
        public string Generate(JamTallyNewResult result)
        {
            var sb = new StringBuilder();

            GenerateHeader(sb);
            GenerateRanking(result, sb);
            GenerateAwards(result, sb);
            GenerateFooter(sb);

            return sb.ToString();
        }

        private void GenerateHeader(StringBuilder sb)
        {
            sb.AppendLine("[CENTER][IMG]<JAM LOGO URL>[/IMG]");
            sb.AppendLine("");
            sb.AppendLine("__________________________________");
            sb.AppendLine("");
            sb.AppendLine("[IMG]<TOP 3 URL>[/IMG]");
            sb.AppendLine("___________");
            sb.AppendLine("");
            sb.AppendLine("[IMG]<BEST THEME URL>[/IMG]");
            sb.AppendLine("");
            sb.AppendLine("[IMG]<BEST CONCEPT URL>[/IMG]");
            sb.AppendLine("");
            sb.AppendLine("[IMG]<BEST STORY URL>[/IMG]");
            sb.AppendLine("");
            sb.AppendLine("[IMG]<BEST VISUALS URL>[/IMG]");
            sb.AppendLine("");
            sb.AppendLine("[IMG]<BEST AUDIO URL>[/IMG]");
            sb.AppendLine("");
            sb.AppendLine("[IMG]<BEST UX URL>[/IMG]");
            sb.AppendLine("");
            sb.AppendLine("[IMG]<BEST REVIEWER URL>[/IMG]");
            sb.AppendLine("___________");
            sb.AppendLine("");
            sb.AppendLine("[SIZE=6]All medals for all participants are available [URL='<MEDALS ZIP URL>']HERE[/URL].[/SIZE]");
            sb.AppendLine("They are available in both 120px-tall versions (suitable to use directly in signatures) and 240px versions.");
            sb.AppendLine("Also, for games with best-of awards there are versions with a rank, a best-of and both.");
            sb.AppendLine("[/CENTER]");
            sb.AppendLine("");
        }

        private void GenerateRanking(JamTallyNewResult result, StringBuilder sb)
        {
            sb.AppendLine("[SIZE=7]Full Rankings[/SIZE]");
            sb.AppendLine("");
            sb.AppendLine("[SPOILER=Images Ranking][IMG]<FULL RANKING URL>[/IMG][/SPOILER]");
            sb.AppendLine("");
            sb.AppendLine("[LIST=1]");
            foreach (var entryScore in result.Ranking)
            {
                sb.Append("[*]");
                PrintEntry(entryScore.Entry, sb);
            }
            sb.AppendLine("[/LIST]");
            sb.AppendLine("");
        }

        private void GenerateAwards(JamTallyNewResult result, StringBuilder sb)
        {
            sb.AppendLine("[SIZE=7]Awards[/SIZE]");
            sb.AppendLine("");
            foreach (var kvp in result.AwardEntries)
            {
                var awardCriterion = kvp.Key;
                var winners = kvp.Value;
                GenerateAwardWinners(awardCriterion, winners, sb);
            }

            sb.AppendLine($"[SIZE=6]Best Reviewer:[/SIZE]");
            foreach (var reviewer in result.TopReviewers)
            {
                sb.AppendLine($"[B]{reviewer}[/B]");
            }
            sb.AppendLine($"");
        }

        private void GenerateAwardWinners(JamAwardCriterion awardCriterion, IEnumerable<JamEntry> winners, StringBuilder sb)
        {
            sb.AppendLine($"[SIZE=6]{awardCriterion.Name}:[/SIZE]");
            foreach (var winner in winners)
            {
                PrintEntry(winner, sb);
            }
            sb.AppendLine($"");
        }

        private void PrintEntry(JamEntry entry, StringBuilder sb)
        {
            var team = entry.RawTeam;
            var authors = string.Join(", ", entry.Authors);

            if (team != null)
                sb.AppendLine($"[B]{entry.Title}[/B] by {team} ({authors})");
            else
                sb.AppendLine($"[B]{entry.Title}[/B] by {authors}");
        }

        private void GenerateFooter(StringBuilder sb)
        {
            sb.AppendLine("[SIZE=7]Results Spreadsheet[/SIZE]");
            sb.AppendLine("");
            sb.AppendLine("[URL]<SPREADSHEET URL; REMEMBER TO MAKE IT PUBLIC!>[/URL]");
        }
    }
}
