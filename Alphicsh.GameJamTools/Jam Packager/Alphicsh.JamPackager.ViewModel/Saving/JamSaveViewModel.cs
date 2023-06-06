using Alphicsh.JamTools.Common.Mvvm.Saving;
using Alphicsh.JamPackager.Model.Jam.Saving;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamPackager.ViewModel.Jam;

namespace Alphicsh.JamPackager.ViewModel.Saving
{
    public class JamSaveViewModel : SaveViewModel<JamEditable, JamEditableViewModel>
    {
        public JamSaveViewModel() : base(
            new JamSaveModel(),
            new JamSaveDataObserver()
            )
        {
        }
    }
}
