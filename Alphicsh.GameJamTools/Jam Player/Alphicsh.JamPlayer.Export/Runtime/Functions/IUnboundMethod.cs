namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public interface IUnboundMethod
    {
        IPrototype ReturnType { get; }
        IFunction Bind(IInstance instance);
    }
}