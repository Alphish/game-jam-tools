using Alphicsh.JamPackager.ViewModel.Jam;
using Alphicsh.JamTools.Common.Mvvm.Observation;
using Alphicsh.JamTools.Common.Mvvm.Saving;

namespace Alphicsh.JamPackager.ViewModel.Saving
{
    internal class JamSaveDataObserver : SaveDataObserver<JamEditableViewModel>
    {
        // changes detection is incomplete yet
        // the tool is for ultimately me, and I don't need it very urgently

        protected override IObserverNode CreateInnerObserver(JamEditableViewModel viewModel)
        {
            // no advanced 
            return CreateViewModelObserver()
                .ObservingProperty(viewModel.TitleProperty)
                .ObservingProperty(viewModel.ThemeProperty)
                .ObservingCollection(viewModel.Awards, CreateAwardObserver);
        }

        private IObserverNode CreateAwardObserver(JamAwardEditableViewModel viewModel)
        {
            return CreateViewModelObserver()
                .ObservingProperty(viewModel.IdProperty)
                .ObservingProperty(viewModel.NameProperty)
                .ObservingProperty(viewModel.DescriptionProperty);
        }
    }
}
