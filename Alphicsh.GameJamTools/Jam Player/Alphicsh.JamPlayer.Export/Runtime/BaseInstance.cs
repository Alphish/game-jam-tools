using System.Collections.Generic;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public class BaseInstance<TInstance> : IInstance<TInstance>
        where TInstance : IInstance<TInstance>
    {
        public IPrototype<TInstance> Prototype { get; }

        IPrototype IInstance.Prototype => this.Prototype;

        public BaseInstance(IPrototype<TInstance> prototype)
        {
            Prototype = prototype;
        }

        public IInstance GetMember(CodeName codeName)
            => Prototype.GetMember(this, codeName);
        public IInstance GetItem(int index)
            => Prototype.GetItem(this, index);
        public IInstance Call(IEnumerable<IInstance> arguments)
            => Prototype.Call(this, arguments);
    }
}