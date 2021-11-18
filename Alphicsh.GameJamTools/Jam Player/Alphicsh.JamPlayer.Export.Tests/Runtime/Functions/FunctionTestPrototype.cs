using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Functions
{
    public class FunctionTestPrototype : IPrototype
    {
        // --------
        // Creation
        // --------
        
        public CodeName Name { get; }
        private HashSet<CodeName> SupertypeNames { get; }

        private FunctionTestPrototype(string prototypeName, IEnumerable<string> supertypeNames)
        {
            Name = CodeName.From(prototypeName);
            SupertypeNames = supertypeNames.Select(CodeName.From).ToHashSet();
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
    }
}