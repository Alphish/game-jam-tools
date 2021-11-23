using System.Collections.Generic;
using Alphicsh.JamPlayer.Export.Runtime.Functions;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class VariableDeclarationScope : IVariableDeclarationScope
    {
        private IVariableDeclarationScope? ParentScope { get; }
        private IDictionary<CodeName, VariableDeclaration> Declarations { get; }

        public VariableDeclarationScope() : this(parentScope: null) { }

        private VariableDeclarationScope(IVariableDeclarationScope? parentScope)
        {
            ParentScope = parentScope;
            Declarations = new Dictionary<CodeName, VariableDeclaration>();
        }

        public void DeclareVariable(VariableDeclaration declaration)
        {
            Declarations.Add(declaration.Name, declaration);
        }

        public VariableDeclaration? GetDeclaration(CodeName variableName)
        {
            if (Declarations.TryGetValue(variableName, out var ownDeclaration))
                return ownDeclaration;
            else if (ParentScope != null)
                return ParentScope.GetDeclaration(variableName);
            else
                return null;
        }

        public IVariableDeclarationScope CreateChildScope()
        {
            return new VariableDeclarationScope(this);
        }
    }
}