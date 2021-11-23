namespace Alphicsh.JamPlayer.Export.Runtime.Numbers
{
    public class NumberPrototype : BasePrototype<NumberInstance>
    {
        // --------
        // Creation
        // --------

        static NumberPrototype()
        {
            Prototype = new NumberPrototype();
            Prototype.Init();
        }
        
        public static NumberPrototype Prototype { get; }
        private NumberPrototype() { }
        private void Init()
        {
        }
        
        // ---------
        // Overrides
        // ---------

        public override TypeName Name { get; } = TypeName.CreateSimple("Number");
    }
}