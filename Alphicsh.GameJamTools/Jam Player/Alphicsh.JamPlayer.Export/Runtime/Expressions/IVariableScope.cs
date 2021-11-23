using Alphicsh.JamPlayer.Export.Runtime.Functions;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public interface IVariableScope
    {
        void DefineVariable(VariableDeclaration declaration, IInstance instance);
        IInstance? GetVariableValue(CodeName variableName);
        IVariableScope CreateChildScope();
    }
}