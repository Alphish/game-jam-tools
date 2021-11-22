namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public static class FunctionExtensions
    {
        public static IInstance ToFunctionInstance(this IFunction function)
        {
            return new FunctionInstance(function);
        }
    }
}