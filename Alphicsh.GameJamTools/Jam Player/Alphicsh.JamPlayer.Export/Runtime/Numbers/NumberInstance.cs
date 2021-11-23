namespace Alphicsh.JamPlayer.Export.Runtime.Numbers
{
    public class NumberInstance : BaseInstance<NumberInstance>
    {
        public double InnerValue { get; }
        
        public NumberInstance(double innerValue)
            : base(NumberPrototype.Prototype)
        {
            InnerValue = innerValue;
        }
    }
}