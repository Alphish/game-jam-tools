using System;
using System.Windows.Media;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public class JamAlignmentViewModel : WrapperViewModel<JamAlignment>
    {
        public JamAlignmentViewModel(JamAlignment model)
            : base(model)
        {
            Title = model.Title;

            if (model.Color != null)
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(model.Color);
                    AlignmentBrush = new SolidColorBrush(color);
                }
                catch (FormatException)
                {
                    AlignmentBrush = null;
                }
            }
        }

        public string Title { get; }
        public Brush? AlignmentBrush { get; }
    }
}
