using Alphicsh.JamPlayer.Export.Runtime.Functions;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public interface IVariableDeclarationScope
    {
        void DeclareVariable(VariableDeclaration declaration);
        VariableDeclaration? GetDeclaration(CodeName variableName);
        IVariableDeclarationScope CreateChildScope();
    }
}