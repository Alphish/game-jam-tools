namespace Alphicsh.JamPlayer.Export.Runtime
{
    public interface IPrototype
    {
        CodeName Name { get; }
        
        bool IsSubtypeOf(IPrototype matchedType);
    }
}
