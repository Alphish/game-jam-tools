namespace Alphicsh.JamPlayer.Export.Runtime.Common
{
    public class TestDummyInstance : BaseInstance<TestDummyInstance>
    {
        public TestDummyInstance() : base(TestDummyPrototype.Prototype)
        {
        }
    }
}