using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using System.Windows.Media;

namespace Alphicsh.JamTally.ViewModel.Jam
{
    public class JamAlignmentOptionViewModel : BaseViewModel
    {
        public JamAlignmentOptionViewModel(JamAlignmentOption? option, string title)
        {
            Option = option;
            Title = title;
            Color = option?.Color != null ? (Color)ColorConverter.ConvertFromString(option.Color) : null;
            Brush = Color.HasValue ? new SolidColorBrush(Color.Value) : null;
        }

        public JamAlignmentOption? Option { get; }
        public string Title { get; }
        public Color? Color { get; }
        public Brush? Brush { get; }
    }
}
