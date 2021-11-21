using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionTestInstance : IInstance
    {
        public IPrototype Prototype { get; }

        public FunctionTestInstance(IPrototype prototype)
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