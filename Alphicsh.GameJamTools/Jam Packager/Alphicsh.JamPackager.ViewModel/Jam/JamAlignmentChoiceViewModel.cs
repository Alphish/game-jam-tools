using System.Windows.Media;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamPackager.ViewModel.Jam
{
    public class JamAlignmentChoiceViewModel : BaseViewModel
    {
        public JamAlignmentChoiceViewModel(JamAlignmentEditable? option, string title)
        {
            Id = option?.Id;
            Title = title;
            Color = option?.Color != null ? (Color)ColorConverter.ConvertFromString(option.Color) : null;
            Brush = Color.HasValue ? new SolidColorBrush(Color.Value) : null;
        }

        public string? Id { get; }
        public string Title { get; }
        public Color? Color { get; }
        public Brush? Brush { get; }
    }
}
