using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public class SimpleTestInstance : IInstance
    {
        public IPrototype Prototype { get; }

        public SimpleTestInstance(IPrototype prototype)
        {
            Prototype = prototype;
        }
        
        [ExcludeFromCodeCoverage] public IInstance GetMember(CodeName codeName)
            => throw new NotSupportedException();
        [ExcludeFromCodeCoverage] public IInstance GetItem(int index)
            => throw new NotSupportedException();
        [ExcludeFromCodeCoverage] public IInstance Call(IEnumerable<IInstance> arguments)
            => throw new NotSupportedException();
    }
}