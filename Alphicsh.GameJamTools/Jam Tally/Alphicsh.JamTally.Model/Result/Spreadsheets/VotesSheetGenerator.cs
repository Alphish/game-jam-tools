using System.Linq;
using Alphicsh.JamTally.Model.Vote;

namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    internal class VotesSheetGenerator
    {
        public string Generate(JamTallyResult tallyResult)
        {
            var sheet = GenerateSheet(tallyResult);
            return sheet.GenerateCsv();
        }

        private Sheet GenerateSheet(JamTallyResult tallyResult)
        {
            var sheet = new Sheet();
            sheet.AddColumn(MakeStartingColumn(tallyResult));
            for (var i = 0; i < tallyResult.Votes.Count; i++)
            {
                var vote = tallyResult.Votes.ElementAt(i);
                sheet.AddColumn(MakeVoteMainColumn(tallyResult, vote));
                sheet.AddColumn(MakeVoteAuxiliaryColumn(tallyResult, i, vote));
            }
            return sheet;
        }

        // -------
        // Initial
        // -------

        private SheetColumn MakeStartingColumn(JamTallyResult tallyResult)
        {
            var column = new SheetColumn();
            column.Add("Rank\\Voter");

            for (var i = 1; i <= tallyResult.EntriesCount; i++)
            {
                column.Add(i.ToString());
            }

            foreach (var award in tallyResult.Awards)
            {
                column.Add(award.Name);
            }

            for (var i = 0; i < tallyResult.UnjudgedMaxCount; i++)
            {
                if (i == 0)
                    column.Add("Unjudged entries");
                else
                    column.Add("");
            }

            for (var i = 0; i < tallyResult.ReactionsMaxCount; i++)
            {
                if (i == 0)
                    column.Add("Reactions");
                else
                    column.Add("");
            }

            column.Add(""); // for totals

            return column;
        }

        // -----
        // Votes
        // -----

        private SheetColumn MakeVoteMainColumn(JamTallyResult tallyResult, JamVote vote)
        {
            var column = new SheetColumn();
            column.Add(vote.Voter ?? "<voter>");

            // adding ranking entries
            var i = 0;
            foreach (var entry in vote.Ranking)
            {
                column.Add(entry.Title);
                i++;
            }
            while (i < tallyResult.EntriesCount)
            {
                column.Add("");
                i++;
            }

            // adding award entries
            foreach (var award in tallyResult.Awards)
            {
                var entry = vote.FindEntryForAward(award);
                column.Add(entry?.Title ?? "");
            }

            // adding unjudged entries
            var j = 0;
            foreach (var entry in vote.Unjudged)
            {
                column.Add(entry.Title);
                j++;
            }
            while (j < tallyResult.UnjudgedMaxCount)
            {
                column.Add("");
                j++;
            }

            // adding reaction names
            var r = 0;
            foreach (var reaction in vote.Reactions)
            {
                column.Add(reaction.Name);
                r++;
            }
            while (r < tallyResult.ReactionsMaxCount)
            {
                column.Add("");
                r++;
            }

            column.Add("Total");

            return column;
        }

        private SheetColumn MakeVoteAuxiliaryColumn(JamTallyResult tallyResult, int voteIndex, JamVote vote)
        {
            var columnIndex = 3 + 2 * voteIndex;
            var column = new SheetColumn();
            column.Add("");

            // adding check lookups for ranking, awards and unjudged entries
            var lookupsCount = tallyResult.EntriesCount + tallyResult.AwardsCount + tallyResult.UnjudgedMaxCount;
            var lookupRange = Sheet.At(1, 2, true, true) + ":" + Sheet.At(2, 1 + tallyResult.EntriesCount, true, true);

            for (var i = 0; i < lookupsCount; i++)
            {
                var rowIndex = i + 2;
                var titleCell = Sheet.At(columnIndex - 1, rowIndex);
                column.Add($"=IF({titleCell}=\"\",,VLOOKUP({titleCell}, Ranking!{lookupRange}, 2, FALSE))");
            }

            // adding reaction scores
            var r = 0;
            foreach (var reaction in vote.Reactions)
            {
                column.Add(reaction.Value.ToString());
                r++;
            }
            while (r < tallyResult.ReactionsMaxCount)
            {
                column.Add("");
                r++;
            }

            var reactionsStartCell = Sheet.At(columnIndex, 2 + lookupsCount);
            var reactionsEndCell = Sheet.At(columnIndex, 1 + lookupsCount + tallyResult.ReactionsMaxCount);
            column.Add($"=SUM({reactionsStartCell}:{reactionsEndCell})");

            return column;
        }
    }
}
