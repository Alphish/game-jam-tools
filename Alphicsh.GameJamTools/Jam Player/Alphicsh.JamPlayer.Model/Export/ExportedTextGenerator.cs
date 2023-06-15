using System.Linq;
using System.Text;
using Alphicsh.JamPlayer.Model.Awards;
using Alphicsh.JamPlayer.Model.Ranking;

namespace Alphicsh.JamPlayer.Model.Export
{
    internal class ExportedTextGenerator
    {
        public string GenerateExportedText(AppModel appModel, ExportOptions options)
        {
            var sb = new StringBuilder();
            GenerateTitle(sb, options.ReviewsTitle);

            var areCompleteRankings = !appModel.Ranking.PendingEntries.Any();
            if (areCompleteRankings || options.ExportIncompleteRankings)
            {
                GenerateTop3(sb, appModel.Ranking);
                GenerateAwards(sb, appModel.Awards);
                GenerateRanking(sb, appModel.Ranking);
            }

            GenerateComments(sb, appModel.Ranking, options.EntryCommentTemplate);
            return sb.ToString();
        }

        private void GenerateTitle(StringBuilder sb, string reviewsTitle)
        {
            sb.Append($"[center][b][size=7]{ reviewsTitle }[/size][/b][/center]");
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
                sb.Append($"[size=5][b]{ jamEntry.Title }[/b] by { jamEntry.Team.Description }[/size]\n");
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
                sb.Append($"[b]{ awardEntry.Criterion.Name }:[/b] { jamEntry.Title } by { jamEntry.Team.Description }");
            }
        }

        private void GenerateRanking(StringBuilder sb, RankingOverview ranking)
        {
            if (!ranking.RankedEntries.Any())
                return;

            sb.Append("\n\n");
            sb.Append($"[size=6]Ranking:[/size]");

            sb.Append("\n\n");
            sb.Append($"[spoiler]");

            GenerateRankedList(sb, ranking);
            GenerateUnjudgedList(sb, ranking);

            sb.Append("[/spoiler]");
        }

        private void GenerateRankedList(StringBuilder sb, RankingOverview ranking)
        {
            sb.Append($"[list=1]");
            foreach (var rankingEntry in ranking.RankedEntries)
            {
                var jamEntry = rankingEntry.JamEntry;
                sb.Append("\n");
                sb.Append($"[*][b]{jamEntry.ShortTitle}[/b] by {jamEntry.Team.Description}");
            }
            sb.Append("[/list]");
        }

        private void GenerateUnjudgedList(StringBuilder sb, RankingOverview ranking)
        {
            var pendingEntries = ranking.PendingEntries.Select(entry => entry.JamEntry);
            var markedEntries = ranking.GetAllEntries().Where(entry => entry.IsUnjudged).Select(entry => entry.JamEntry);
            var rankedEntries = ranking.RankedEntries.Select(entry => entry.JamEntry);

            var unjudgedEntries = pendingEntries.Concat(markedEntries).Except(rankedEntries)
                .OrderBy(entry => entry.ShortTitle).ThenBy(entry => entry.Team.Description)
                .ToList();

            if (unjudgedEntries.Count == 0)
                return;

            sb.Append("\n\n");
            sb.Append($"[b]Unjudged entries:[/b]");
            sb.Append("\n\n");

            sb.Append($"[list]");
            foreach (var jamEntry in unjudgedEntries)
            {
                sb.Append("\n");
                sb.Append($"[*][b]{jamEntry.ShortTitle}[/b] by {jamEntry.Team.Description}");
            }
            sb.Append("[/list]");
        }

        private void GenerateComments(StringBuilder sb, RankingOverview ranking, string commentTemplateString)
        {
            var entriesWithComments = ranking.RankedEntries.Concat(ranking.UnrankedEntries)
                .Where(rankingEntry => !string.IsNullOrWhiteSpace(rankingEntry.Comment))
                .OrderBy(rankingEntry => rankingEntry.JamEntry.Title)
                .ThenBy(rankingEntry => rankingEntry.JamEntry.Team.Description)
                .ToList();

            if (!entriesWithComments.Any())
                return;

            sb.Append("\n\n");
            sb.Append("[size=6]Comments:[/size]");

            sb.Append("\n\n");
            sb.Append("[spoiler]");

            var entryTemplate = EntryTemplateParser.Parse(commentTemplateString);
            foreach (var rankingEntry in entriesWithComments)
            {
                sb.Append("\n");
                sb.Append(entryTemplate.FormatEntry(rankingEntry));
            }

            sb.Append("[/spoiler]");
        }
    }
}
