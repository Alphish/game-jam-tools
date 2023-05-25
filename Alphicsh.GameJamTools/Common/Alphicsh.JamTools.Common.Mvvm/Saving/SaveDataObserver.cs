using Alphicsh.JamTools.Common.Mvvm.Observation;

namespace Alphicsh.JamTools.Common.Mvvm.Saving
{
    public abstract class SaveDataObserver<TViewModel> : TreeObserver
    {
        private ISaveViewModel SaveViewModel { get; set; } = default!;

        internal void SetViewModel(ISaveViewModel saveViewModel)
        {
            SaveViewModel = saveViewModel;
        }

        public void ObserveViewModel(TViewModel viewModel)
        {
            Unobserve();
            ClearObservers();

            var newObserver = CreateInnerObserver(viewModel);
            AddObserver(newObserver);
            Observe();
        }

        protected abstract IObserverNode CreateInnerObserver(TViewModel viewModel);

        protected override void OnObservation()
        {
            SaveViewModel.UpdateCurrentData();
        }
    }
}
