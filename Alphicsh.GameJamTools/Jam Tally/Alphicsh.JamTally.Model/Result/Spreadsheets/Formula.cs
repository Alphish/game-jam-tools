namespace Alphicsh.JamTally.Model.Result.Spreadsheets
{
    public static class Formula
    {
        // -----------
        // Expressions
        // -----------

        public static string CountIfExpression(string range, string condition)
            => $"COUNTIF({range},{condition})";

        public static string CountIfEqualExpression(string range, string value)
            => CountIfExpression(range, "\"=\"&" + value);

        public static string SumIfExpression(string referenceRange, string condition, string valueRange)
            => $"SUMIF({referenceRange},{condition},{valueRange})";

        public static string VlookupExpression(string referenceCell, string range, string index)
            => $"VLOOKUP({referenceCell},{range},{index},FALSE)";

        // --------
        // Formulas
        // --------

        public static string CountIfOrEmptyFormula(string range, string condition)
        {
            var countifExpression = CountIfExpression(range, condition);
            var formula = $"=IF({countifExpression}>0,{countifExpression},)";
            return formula;
        }
    }
}
