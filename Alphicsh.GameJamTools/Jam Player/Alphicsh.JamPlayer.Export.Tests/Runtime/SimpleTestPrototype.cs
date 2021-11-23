using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public class SimpleTestPrototype : IPrototype
    {
        // --------
        // Creation
        // --------
        
        public TypeName Name { get; }
        private HashSet<TypeName> SupertypeNames { get; }

        private SimpleTestPrototype(string prototypeName, IEnumerable<string> supertypeNames)
        {
            Name = TypeName.CreateSimple(prototypeName);
            SupertypeNames = supertypeNames.Select(TypeName.CreateSimple).ToHashSet();
        }

        public static SimpleTestPrototype Lorem { get; }
            = new SimpleTestPrototype("Lorem", Enumerable.Empty<string>());
        public static SimpleTestPrototype Ipsum { get; }
            = new SimpleTestPrototype("Ipsum", Enumerable.Empty<string>());
        public static SimpleTestPrototype SubIpsum { get; }
            = new SimpleTestPrototype("SubIpsum", supertypeNames: new string[] { "Ipsum" });
        
        public bool IsSubtypeOf(IPrototype matchedType)
        {
            return matchedType == this || SupertypeNames.Contains(matchedType.Name);
        }

        public IInstance CreateInstance()
        {
            return new SimpleTestInstance(this);
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