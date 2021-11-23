using System.Collections.Generic;
using Alphicsh.JamPlayer.Export.Runtime.Numbers;

namespace Alphicsh.JamPlayer.Export.Runtime.Strings
{
    public class StringPrototype : BasePrototype<StringInstance>
    {
        // --------
        // Creation
        // --------

        static StringPrototype()
        {
            Prototype = new StringPrototype();
            Prototype.Init();
        }
        
        public static StringPrototype Prototype { get; }
        private StringPrototype() { }
        private void Init()
        {
            DefineGetter("length")
                .Returning(NumberPrototype.Prototype)
                .Executing(StringPrototype.GetLength)
                .Complete();
            
            DefineMethod("toUpper").WithNoParameters()
                .Returning(StringPrototype.Prototype)
                .Executing(StringPrototype.ToUpper)
                .Complete();
                
            DefineMethod("toLower").WithNoParameters()
                .Returning(StringPrototype.Prototype)
                .Executing(StringPrototype.ToLower)
                .Complete();
        }

        // ---------
        // Overrides
        // ---------

        public override TypeName Name { get; } = TypeName.CreateSimple("String");
        
        // -------
        // Getters
        // -------

        public static IInstance GetLength(IInstance instance)
        {
            var stringInstance = (StringInstance)instance;
            return new NumberInstance(stringInstance.InnerString.Length);
        }
        
        // -------
        // Methods
        // -------

        public static IInstance ToUpper(IInstance instance, IEnumerable<IInstance> arguments)
        {
            var stringInstance = (StringInstance)instance;
            return new StringInstance(stringInstance.InnerString.ToUpper());
        }
        
        public static IInstance ToLower(IInstance instance, IEnumerable<IInstance> arguments)
        {
            var stringInstance = (StringInstance)instance;
            return new StringInstance(stringInstance.InnerString.ToLower());
        }
    }
}