using System.Windows;

namespace Alphicsh.JamTools.Common.Theming
{
    internal class ThemeVariable<TValue>
    {
        private ResourceDictionary Dictionary { get; }
        private string Key { get; }

        public ThemeVariable(ResourceDictionary dictionary, string key)
        {
            Dictionary = dictionary;
            Key = key;
        }

        public TValue Value
        {
            get => (TValue)Dictionary[Key];
            set => Dictionary[Key] = value;
        }
    }
}
