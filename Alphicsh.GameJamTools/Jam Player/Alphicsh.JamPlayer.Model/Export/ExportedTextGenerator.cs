using System.Linq;
using System.Text;
using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.Model.Export
{
    internal class ExportedTextGenerator
    {
        public string GenerateExportedText(AppModel appModel)
        {
            var sb = new StringBuilder();
            GenerateTitle(sb);
            GenerateTop3(sb, appModel.Ranking);
            GenerateAwards(sb, appModel.Awards);
            GenerateRanking(sb, appModel.Ranking);
            GenerateComments(sb, appModel.Ranking);
            return sb.ToString();
        }

        private void GenerateTitle(StringBuilder sb)
        {
            sb.Append("[center][b][size=7]Reviews[/size][/b][/center]");
        }

        private void GenerateTop3(StringBuilder sb, RankingOverview ranking)
        {
            if (!ranking.RankedEntries.Any())
                return;

            sb.Append("\n\n");
            sb.Append("[size=6]Top 3:[/size]");

            var top3Entries = ranking.RankedEntries.Take(3);
            foreach (var rankingEntry in top3Entries)
            {
                var jamEntry = rankingEntry.JamEntry;
                sb.Append("\n\n");
                sb.Append($"[size=5][b]{ jamEntry.Title }[/b] by { jamEntry.Team.Description }\n");
                sb.Append("\n");
                sb.Append(rankingEntry.Comment);
            }
        }

        private void GenerateAwards(StringBuilder sb, AwardsOverview awards)
        {
            var awardEntries = awards.Entries.Where(entry => entry.JamEntry != null).ToList();
            if (!awardEntries.Any())
                return;

            sb.Append("\n\n");
            sb.Append("[size=6]Best ofs:[/size]");
            sb.Append("\n");

            foreach (var awardEntry in awardEntries)
            {
                var jamEntry = awardEntry.JamEntry!;
                sb.Append("\n");
                sb.Append($"[b]{ awardEntry.Criterion.Description }:[/b] { jamEntry.Title } by { jamEntry.Team.Description }\n");
            }
        }

        private void GenerateRanking(StringBuilder sb, RankingOverview ranking)
        {
            if (!ranking.RankedEntries.Any())
                return;

            var isIncompleteRanking = ranking.PendingEntries.Any();

            var heading = isIncompleteRanking ? "Ranking so far" : "Full ranking";
            sb.Append("\n\n");
            sb.Append($"[size=6]{ heading }:[/size]");

            var listTag = isIncompleteRanking ? "[list]" : "[list=1]";
            sb.Append("\n\n");
            sb.Append($"[spoiler]{ listTag }");

            foreach (var rankingEntry in ranking.RankedEntries)
            {
                var jamEntry = rankingEntry.JamEntry;
                sb.Append("\n");
                sb.Append($"[*][b]{ jamEntry.Title } by { jamEntry.Team.Description }");
            }

            sb.Append("\n");
            sb.Append("[/list][/spoiler]");
        }

        private void GenerateComments(StringBuilder sb, RankingOverview ranking)
        {
            var entriesWithComments = ranking.RankedEntries.Concat(ranking.UnrankedEntries)
                .Where(rankingEntry => !string.IsNullOrWhiteSpace(rankingEntry.Comment))
                .ToList();

            if (!entriesWithComments.Any())
                return;

            sb.Append("\n\n");
            sb.Append("[size=6]Comments:[/size]");

            sb.Append("\n\n");
            sb.Append("[spoiler]");

            foreach (var rankingEntry in entriesWithComments)
            {
                sb.Append("\n");
                GenerateSingleComment(sb, rankingEntry);
                sb.Append("\n");
            }

            sb.Append("[/spoiler]");
        }

        private void GenerateSingleComment(StringBuilder sb, RankingEntry rankingEntry)
        {
            var jamEntry = rankingEntry.JamEntry;
            sb.Append($"[b]{ jamEntry.Title }[/b] by { jamEntry.Team.Description }");
            sb.Append($"\n\n");
            sb.Append(rankingEntry.Comment.Replace("\r\n", "\n"));
        }
    }
}
