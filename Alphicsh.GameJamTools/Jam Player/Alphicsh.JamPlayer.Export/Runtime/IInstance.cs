using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public interface IInstance
    {
        IPrototype Prototype { get; }
        
        IInstance GetMember(CodeName codeName);
        IInstance GetItem(int index);
        IInstance Call(IEnumerable<IInstance> arguments);
    }

    public interface IInstance<TInstance> : IInstance
        where TInstance : IInstance<TInstance>
    {
        new IPrototype<TInstance> Prototype { get; }
    }
}
