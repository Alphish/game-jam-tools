using System;
using Alphicsh.JamPlayer.Export.Runtime.Functions;

namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class RuntimeVariable
    {
        public VariableDeclaration Declaration { get; }
        public IInstance Instance { get; }

        public RuntimeVariable(VariableDeclaration declaration, IInstance instance)
        {
            if (!instance.Prototype.IsSubtypeOf(declaration.Prototype))
            {
                throw new ArgumentException(
                    $"The value must match the '{declaration.Prototype.Name} {declaration.Name}' variable declaration.",
                    nameof(instance)
                    );
            }
            
            Declaration = declaration;
            Instance = instance;
        }
    }
}