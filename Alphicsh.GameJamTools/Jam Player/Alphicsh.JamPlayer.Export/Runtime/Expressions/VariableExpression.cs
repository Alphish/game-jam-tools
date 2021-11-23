namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class VariableExpression : IExpression
    {
        // --------
        // Creation
        // --------
        
        private VariableDeclaration Declaration { get; }

        private VariableExpression(VariableDeclaration declaration)
        {
            Declaration = declaration;
        }

        public static VariableExpression Create(IVariableDeclarationScope declarationScope, CodeName variableName)
        {
            var declaration = declarationScope.GetDeclaration(variableName);
            if (declaration == null)
            {
                var message = $"Cannot create a variable expression for an unknown variable '{variableName}'.";
                throw new ExpressionCompilationException(message);
            }

            return new VariableExpression(declaration);
        }
        
        // ----------
        // Evaluation
        // ----------

        public IPrototype EvaluatedType
            => Declaration.Prototype;

        public IInstance Evaluate(IVariableScope variableScope)
            => variableScope.GetVariableValue(Declaration.Name)!;
    }
}