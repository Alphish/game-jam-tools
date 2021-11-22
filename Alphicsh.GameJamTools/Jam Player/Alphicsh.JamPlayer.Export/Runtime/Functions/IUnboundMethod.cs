namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public interface IUnboundMethod
    {
        FunctionParameterList Parameters { get; }
        IPrototype ReturnType { get; }
        IFunction Bind(IInstance instance);
    }
}