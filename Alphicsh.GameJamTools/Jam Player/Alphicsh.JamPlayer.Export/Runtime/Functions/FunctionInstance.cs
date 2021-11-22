namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionInstance : BaseInstance<FunctionInstance>
    {
        public IFunction InnerFunction { get; }

        public FunctionInstance(IFunction innerFunction)
            : base(FunctionPrototype.MatchingFunction(innerFunction))
        {
            InnerFunction = innerFunction;
        }
    }
}