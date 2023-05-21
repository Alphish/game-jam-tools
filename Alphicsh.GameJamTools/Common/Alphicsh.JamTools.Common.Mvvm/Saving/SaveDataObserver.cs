using System;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.Mvvm.Saving
{
    public abstract class SaveDataObserver<TViewModel>
    {
        private ISaveViewModel SaveViewModel { get; set; } = default!;

        internal void SetViewModel(ISaveViewModel saveViewModel)
        {
            SaveViewModel = saveViewModel;
        }

        public abstract void ObserveViewModel(TViewModel viewModel);

        protected void ObserveCollection<TItemModel, TItemViewModel>(
            CollectionViewModel<TItemModel, TItemViewModel> collection,
            Action<TItemViewModel> subscriptionFunction
            )
            where TItemViewModel : WrapperViewModel<TItemModel>
        {
            // subscribe current collection elements
            foreach (var viewModel in collection)
            {
                subscriptionFunction(viewModel);
            }

            // ensure subscription of future collection elements
            collection.CollectionChanged += (sender, e) =>
            {
                SaveViewModel.UpdateCurrentData();
                foreach (var viewModel in collection)
                {
                    subscriptionFunction(viewModel);
                }
            };
        }

        protected void ObserveProperty(INotifiableProperty notifiableProperty)
        {
            notifiableProperty.PropertyChanged += (sender, e) => SaveViewModel.UpdateCurrentData();
        }
    }
}
