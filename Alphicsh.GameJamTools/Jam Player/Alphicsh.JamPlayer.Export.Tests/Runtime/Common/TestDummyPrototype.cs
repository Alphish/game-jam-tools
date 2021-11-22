using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Common
{
    public class TestDummyPrototype : BasePrototype<TestDummyInstance>
    {
        // --------
        // Creation
        // --------
        
        static TestDummyPrototype()
        {
            Prototype = new TestDummyPrototype();
            Prototype.Init();
        }
        
        public static TestDummyPrototype Prototype { get; }
        private TestDummyPrototype() { }
        private void Init()
        {
            DefineMethod("getSelf").WithNoParameters()
                .Returning(TestDummyPrototype.Prototype)
                .Executing(TestDummyPrototype.GetSelf)
                .Complete();

            DefineMethod("getArgument")
                .WithParameter("arg", TestDummyPrototype.Prototype)
                .Returning(TestDummyPrototype.Prototype)
                .Executing(TestDummyPrototype.GetArgument)
                .Complete();
        }

        public TestDummyInstance CreateInstance()
            => new TestDummyInstance();

        // ---------
        // Overrides
        // ---------

        public override TypeName Name { get; } = TypeName.CreateSimple("TestDummy");
        
        // -------
        // Methods
        // -------

        private static IInstance GetSelf(IInstance instance, IEnumerable<IInstance> arguments)
        {
            return instance;
        }
        
        private static IInstance GetArgument(IInstance instance, IEnumerable<IInstance> arguments)
        {
            return arguments.First();
        }
    }
}