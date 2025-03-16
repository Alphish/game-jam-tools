using ClosedXML.Excel;

namespace Alphicsh.JamTally.Spreadsheets
{
    public static class TallyStyles
    {
        public static XLColor RankHeader { get; } = XLColor.FromHtml("#6fa8dc");
        public static XLColor RankBody { get; } = XLColor.FromHtml("#9fc5e8");

        public static XLColor VoteHeader { get; } = XLColor.FromHtml("#93c47d");
        public static XLColor VoteBody { get; } = XLColor.FromHtml("#d9ead3");
        public static XLColor VoteBodyAlt { get; } = XLColor.FromHtml("#b6d7a8");

        public static XLColor AwardHeader { get; } = XLColor.FromHtml("#c27ba0");
        public static XLColor AwardBody { get; } = XLColor.FromHtml("#ead1dc");
        public static XLColor AwardBodyStrong { get; } = XLColor.FromHtml("#d5a6bd");

        public static XLColor ScoreHeader { get; } = XLColor.FromHtml("#ffd966");
        public static XLColor ScoreBody { get; } = XLColor.FromHtml("#fff2cc");
        public static XLColor ScoreBodyStrong { get; } = XLColor.FromHtml("#ffe599");

        public static XLColor UnjudgedHeader { get; } = XLColor.FromHtml("#e06666");
        public static XLColor UnjudgedBody { get; } = XLColor.FromHtml("#f4cccc");

        public static XLColor BorderColor { get; } = XLColor.FromArgb(255, 0, 0, 0);
    }
}
