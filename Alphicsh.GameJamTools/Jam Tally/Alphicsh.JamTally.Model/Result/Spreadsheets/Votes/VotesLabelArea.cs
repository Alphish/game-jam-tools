namespace Alphicsh.JamTally.Model.Result.Spreadsheets.Votes
{
    internal class VotesLabelArea : SheetArea<VotesSheet>
    {
        public VotesLabelArea(VotesSheet sheet)
            : base(sheet, sheet.LabelColumn, sheet.HeaderRow, hasRows: false)
        {
        }

        public void Populate()
        {
            var column = CreateEntry();

            column.Cursor.MoveTo(Left, Top);
            column.Add("RANK");

            column.Cursor.MoveTo(Left, Sheet.EntriesFirstRow);
            for (var i = 1; i <= Sheet.EntriesCount; i++)
            {
                column.Add(i.ToString());
            }

            column.Cursor.MoveTo(Left, Sheet.AwardsFirstRow);
            foreach (var award in Sheet.TallyResult.Awards)
            {
                column.Add(award.Name);
            }

            column.Cursor.MoveTo(Left, Sheet.UnjudgedFirstRow);
            column.Add("Unjudged entries");

            column.Cursor.MoveTo(Left, Sheet.ReactionsFirstRow);
            column.Add("Reactions");
        }
    }
}
