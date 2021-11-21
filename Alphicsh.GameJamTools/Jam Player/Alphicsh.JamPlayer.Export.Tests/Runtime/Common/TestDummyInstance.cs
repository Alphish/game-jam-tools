namespace Alphicsh.JamPlayer.Export.Runtime.Common
{
    public class TestDummyInstance : BaseInstance<TestDummyInstance>
    {
        public int Value { get; }
        
        public TestDummyInstance(int value) : base(TestDummyPrototype.Prototype)
        {
            Value = value;
        }
    }
}