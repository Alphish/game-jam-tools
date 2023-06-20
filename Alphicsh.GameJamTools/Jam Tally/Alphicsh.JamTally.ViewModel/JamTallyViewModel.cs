using Alphicsh.JamTally.Model;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamTally.ViewModel
{
    public class JamTallyViewModel : WrapperViewModel<JamTallyModel>
    {
        public static JamTallyViewModel Current => (JamTallyViewModel)AppViewModel.Current;

        public JamTallyViewModel(JamTallyModel model) : base(model)
        {
        }
    }
}
