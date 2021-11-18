namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionParameter
    {
        public CodeName Name { get; }
        public IPrototype Prototype { get; }

        public FunctionParameter(CodeName name, IPrototype prototype)
        {
            Name = name;
            Prototype = prototype;
        }

        public static FunctionParameter Create(string name, IPrototype prototype)
            => new FunctionParameter(CodeName.From(name), prototype);
    }
}
