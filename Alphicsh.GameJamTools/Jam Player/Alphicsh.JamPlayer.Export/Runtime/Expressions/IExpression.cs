namespace Alphicsh.JamPlayer.Export.Runtime.Expressions
{
    public interface IExpression
    {
        IPrototype EvaluatedType { get; }
        IInstance Evaluate(IVariableScope scope);
    }
}