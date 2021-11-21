using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime.Common
{
    public class TestDummyPrototype : BasePrototype<TestDummyInstance>
    {
        // --------
        // Creation
        // --------
        
        public static TestDummyPrototype Prototype { get; } = new TestDummyPrototype();

        private TestDummyPrototype()
        {
        }

        public TestDummyInstance CreateInstance(int value = 0)
            => new TestDummyInstance(value);

        // ---------
        // Overrides
        // ---------

        public override TypeName Name { get; } = TypeName.CreateSimple("TestDummy");
    }
}