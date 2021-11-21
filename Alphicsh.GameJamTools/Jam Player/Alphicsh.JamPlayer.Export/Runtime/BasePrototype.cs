using System;
using System.Collections.Generic;
using Alphicsh.JamPlayer.Export.Runtime.Functions;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public abstract partial class BasePrototype<TInstance> : IPrototype<TInstance>
        where TInstance : IInstance
    {
        public abstract TypeName Name { get; }

        public override string ToString()
            => $"Prototype of {Name}";

        public virtual bool IsSubtypeOf(IPrototype matchedType)
        {
            return matchedType == this;
        }
        
        // -------
        // Members
        // -------
        
        public virtual IPrototype GetMemberType(CodeName memberName)
        {
            throw new NotImplementedException();
        }
        
        public virtual IInstance GetMember(TInstance instance, CodeName memberName)
        {
            throw new NotImplementedException();
        }

        IInstance IPrototype.GetMember(IInstance instance, CodeName memberName)
            => this.GetMember((TInstance) instance, memberName);

        // -----
        // Items
        // -----

        public virtual IPrototype GetItemType()
            => throw new NotSupportedException($"The instances of prototype '{Name}' don't have items.");
        public virtual IInstance GetItem(TInstance instance, int index)
            => throw new NotSupportedException($"The instances of prototype '{Name}' don't have items.");

        IInstance IPrototype.GetItem(IInstance instance, int index)
            => this.GetItem((TInstance) instance, index);
        
        // --------------
        // Function calls
        // --------------

        public virtual IPrototype? GetCallReturnType(IEnumerable<IPrototype> argumentTypes)
            => throw new NotSupportedException($"The instances of prototype '{Name}' cannot be called.");
        public virtual IInstance Call(TInstance instance, IEnumerable<IInstance> arguments)
            => throw new NotSupportedException($"The instances of prototype '{Name}' cannot be called.");

        IInstance IPrototype.Call(IInstance instance, IEnumerable<IInstance> arguments)
            => this.Call((TInstance) instance, arguments);
    }
}
