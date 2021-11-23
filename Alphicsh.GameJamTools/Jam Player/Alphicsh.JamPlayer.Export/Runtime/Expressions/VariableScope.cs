using System.Collections.Generic;
using Alphicsh.JamPlayer.Export.Runtime.Functions;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class VariableScope : IVariableScope
    {
        private IVariableScope? ParentScope { get; }
        private IDictionary<CodeName, RuntimeVariable> VariableValues { get; }

        public VariableScope() : this(parentScope: null) { }

        private VariableScope(IVariableScope? parentScope)
        {
            ParentScope = parentScope;
            VariableValues = new Dictionary<CodeName, RuntimeVariable>();
        }

        public void DefineVariable(VariableDeclaration declaration, IInstance instance)
        {
            var variable = new RuntimeVariable(declaration, instance);
            VariableValues.Add(declaration.Name, variable);
        }

        public IInstance? GetVariableValue(CodeName variableName)
        {
            if (VariableValues.TryGetValue(variableName, out var ownVariable))
                return ownVariable.Instance;
            else if (ParentScope != null)
                return ParentScope.GetVariableValue(variableName);
            else
                return null;
        }

        public IVariableScope CreateChildScope()
        {
            return new VariableScope(this);
        }
    }
}