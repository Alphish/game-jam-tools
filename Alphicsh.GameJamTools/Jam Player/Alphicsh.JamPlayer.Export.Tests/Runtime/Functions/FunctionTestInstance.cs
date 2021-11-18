namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionTestInstance : IInstance
    {
        public IPrototype Prototype { get; }

        public FunctionTestInstance(IPrototype prototype)
        {
            Prototype = prototype;
        }
    }
}