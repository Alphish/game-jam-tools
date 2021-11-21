using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionTestPrototype : IPrototype
    {
        // --------
        // Creation
        // --------
        
        public TypeName Name { get; }
        private HashSet<TypeName> SupertypeNames { get; }

        private FunctionTestPrototype(string prototypeName, IEnumerable<string> supertypeNames)
        {
            Name = TypeName.CreateSimple(prototypeName);
            SupertypeNames = supertypeNames.Select(TypeName.CreateSimple).ToHashSet();
        }

        public static FunctionTestPrototype Lorem { get; }
            = new FunctionTestPrototype("lorem", Enumerable.Empty<string>());
        public static FunctionTestPrototype Ipsum { get; }
            = new FunctionTestPrototype("ipsum", Enumerable.Empty<string>());
        public static FunctionTestPrototype SubIpsum { get; }
            = new FunctionTestPrototype("sub_ipsum", supertypeNames: new string[] { "ipsum" });
        
        public bool IsSubtypeOf(IPrototype matchedType)
        {
            return matchedType == this || SupertypeNames.Contains(matchedType.Name);
        }

        public IInstance CreateInstance()
        {
            return new FunctionTestInstance(this);
        }

        [ExcludeFromCodeCoverage] public IPrototype GetMemberType(CodeName memberName)
            => throw new NotSupportedException();
        [ExcludeFromCodeCoverage] public IInstance GetMember(IInstance instance, CodeName memberName)
            => throw new NotSupportedException();
        [ExcludeFromCodeCoverage] public IPrototype GetItemType()
            => throw new NotSupportedException();
        [ExcludeFromCodeCoverage] public IInstance GetItem(IInstance instance, int index)
            => throw new NotSupportedException();
        [ExcludeFromCodeCoverage] public IPrototype GetCallReturnType(IEnumerable<IPrototype> argumentTypes)
            => throw new NotSupportedException();
        [ExcludeFromCodeCoverage] public IInstance Call(IInstance instance, IEnumerable<IInstance> arguments)
            => throw new NotSupportedException();
    }
}