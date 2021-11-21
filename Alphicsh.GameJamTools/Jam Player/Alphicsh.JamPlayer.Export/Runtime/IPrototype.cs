namespace Alphicsh.JamPlayer.Export.Runtime
{
    public interface IPrototype
    {
        TypeName Name { get; }
        
        bool IsSubtypeOf(IPrototype matchedType);
    }
}
