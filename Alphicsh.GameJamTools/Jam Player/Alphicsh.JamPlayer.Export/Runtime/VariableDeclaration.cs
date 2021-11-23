namespace Alphicsh.JamPlayer.Export.Runtime
{
    public class VariableDeclaration
    {
        public CodeName Name { get; }
        public IPrototype Prototype { get; }

        public VariableDeclaration(CodeName name, IPrototype prototype)
        {
            Name = name;
            Prototype = prototype;
        }

        public static VariableDeclaration Create(string name, IPrototype prototype)
            => new VariableDeclaration(CodeName.From(name), prototype);
    }
}
