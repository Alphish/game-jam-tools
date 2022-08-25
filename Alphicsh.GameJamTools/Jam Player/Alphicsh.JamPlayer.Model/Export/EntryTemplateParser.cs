using System;
using System.Collections.Generic;
using System.Text;
using Alphicsh.JamPlayer.Model.Ranking;
using Alphicsh.JamPlayer.Model.Ratings;

namespace Alphicsh.JamPlayer.Model.Export
{
    using ValueSelector = Func<RankingEntry, string>;

    internal sealed class EntryTemplateParser
    {
        public static EntryTemplate Parse(string templateString)
        {
            var parser = new EntryTemplateParser(templateString);
            return parser.DoParse();
        }

        private string TemplateString { get; }
        private StringBuilder FormatBuilder { get; } = new StringBuilder();
        private List<ValueSelector> ValueSelectors { get; } = new List<ValueSelector>();

        private int Pos { get; set; }
        private int OpeningBraceIndex { get; set; }
        private int ClosingBraceIndex { get; set; }

        private EntryTemplateParser(string templateString)
        {
            TemplateString = templateString.Replace("\r\n", "\n");
        }

        private EntryTemplate DoParse()
        {
            Pos = 0;
            OpeningBraceIndex = TemplateString.IndexOf("{", Pos);
            ClosingBraceIndex = TemplateString.IndexOf("}", Pos);

            while (Pos < TemplateString.Length)
            {
                if (Pos == OpeningBraceIndex)
                    ProcessOpeningBrace();
                else if (Pos == ClosingBraceIndex)
                    ProcessClosingBrace();
                else
                    ProcessTemplateLiteral();
            }

            return new EntryTemplate(FormatBuilder.ToString(), ValueSelectors);
        }

        private void ProcessTemplateLiteral()
        {
            var end = TemplateString.Length;
            if (OpeningBraceIndex != -1)
                end = Math.Min(end, OpeningBraceIndex);
            if (ClosingBraceIndex != -1)
                end = Math.Min(end, ClosingBraceIndex);

            var literalPart = TemplateString.Substring(Pos, end - Pos);
            FormatBuilder.Append(literalPart);

            Pos = end;
        }

        private void ProcessClosingBrace()
        {
            Pos++;
            if (Pos < TemplateString.Length && TemplateString[Pos] == '}')
                Pos++;

            FormatBuilder.Append("}}");
            ClosingBraceIndex = TemplateString.IndexOf("}", Pos);
        }

        private void ProcessOpeningBrace()
        {
            Pos++;
            if (Pos < TemplateString.Length && TemplateString[Pos] == '{')
            {
                Pos++;
                FormatBuilder.Append("{{");
                OpeningBraceIndex = TemplateString.IndexOf("{", Pos);
                return;
            }

            if (ClosingBraceIndex < 0)
            {
                FormatBuilder.Append("{{");
                OpeningBraceIndex = TemplateString.IndexOf("{", Pos);
                return;
            }

            var selectorExpression = TemplateString.Substring(Pos, ClosingBraceIndex - Pos);
            var valueSelector = CreateValueSelector(selectorExpression);
            FormatBuilder.Append("{" + ValueSelectors.Count + "}");
            ValueSelectors.Add(valueSelector);

            Pos = ClosingBraceIndex + 1;
            OpeningBraceIndex = TemplateString.IndexOf("{", Pos);
            ClosingBraceIndex = TemplateString.IndexOf("}", Pos);
        }

        private ValueSelector CreateValueSelector(string selectorExpression)
        {
            selectorExpression = selectorExpression.Trim().ToLower();
            return rankingEntry => StringifyEntryProperty(rankingEntry, selectorExpression);
        }

        private string StringifyEntryProperty(RankingEntry entry, string propertyName)
        {
            var propertyValue = entry.GetProperty(propertyName);
            switch (propertyValue)
            {
                case string str:
                    return str;
                case int integer:
                    return integer.ToString();
                case IRating rating:
                    return rating.HasValue ? rating.Value!.ToString()! : "-";
                default:
                    return propertyName == "rank" ? "-" : string.Empty;
            }
        }
    }
}
