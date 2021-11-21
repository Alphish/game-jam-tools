using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public interface IPrototype
    {
        TypeName Name { get; }
        
        bool IsSubtypeOf(IPrototype matchedType);

        IPrototype GetMemberType(CodeName memberName);
        IInstance GetMember(IInstance instance, CodeName memberName);

        IPrototype GetItemType();
        IInstance GetItem(IInstance instance, int index);

        IPrototype? GetCallReturnType(IEnumerable<IPrototype> argumentTypes);
        IInstance Call(IInstance instance, IEnumerable<IInstance> arguments);
    }

    public interface IPrototype<TInstance> : IPrototype
        where TInstance : IInstance
    {
        IInstance GetMember(TInstance instance, CodeName memberName);
        IInstance GetItem(TInstance instance, int index);
        IInstance Call(TInstance instance, IEnumerable<IInstance> arguments);
    }
}
