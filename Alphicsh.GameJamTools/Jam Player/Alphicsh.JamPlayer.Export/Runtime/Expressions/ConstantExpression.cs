namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public class ConstantExpression : IExpression
    {
        // --------
        // Creation
        // --------
        
        private IPrototype Prototype { get; }
        private IInstance Instance { get; }

        private ConstantExpression(IPrototype prototype, IInstance instance)
        {
            Prototype = prototype;
            Instance = instance;
        }

        public static ConstantExpression Create(IPrototype prototype, IInstance instance)
        {
            if (!instance.Prototype.IsSubtypeOf(prototype))
            {
                throw new ExpressionCompilationException(
                    $"The value must match the '{prototype.Name}' variable declaration."
                );
            }
            
            return new ConstantExpression(prototype, instance);
        }
        
        // ----------
        // Evaluation
        // ----------

        public IPrototype EvaluatedType
            => Prototype;

        public IInstance Evaluate(IVariableScope variableScope)
            => Instance;
    }
}