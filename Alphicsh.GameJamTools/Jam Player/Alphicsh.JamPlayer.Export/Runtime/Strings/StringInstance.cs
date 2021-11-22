namespace Alphicsh.JamPlayer.Export.Runtime.Strings
{
    public class StringInstance : BaseInstance<StringInstance>
    {
        public string InnerString { get; }
        
        public StringInstance(string innerString)
            : base(StringPrototype.Prototype)
        {
            InnerString = innerString;
        }
    }
}