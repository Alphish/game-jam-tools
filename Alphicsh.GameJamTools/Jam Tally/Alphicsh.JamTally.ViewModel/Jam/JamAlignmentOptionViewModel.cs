using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamTally.ViewModel.Jam
{
    public class JamAlignmentOptionViewModel : BaseViewModel
    {
        public JamAlignmentOptionViewModel(JamAlignmentOption? option, string title)
        {
            Option = option;
            Title = title;
        }

        public JamAlignmentOption? Option { get; }
        public string Title { get; }
    }
}
