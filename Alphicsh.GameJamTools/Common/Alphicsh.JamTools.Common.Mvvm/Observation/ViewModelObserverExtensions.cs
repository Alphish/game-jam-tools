using System;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.Mvvm.Observation
{
    public static class ViewModelObserverExtensions
    {
        public static ViewModelObserver ObservingProperty(this ViewModelObserver observer, INotifiableProperty property)
        {
            observer.RegisterProperty(property);
            return observer;
        }

        public static ViewModelObserver ObservingCollection<TItemModel, TItemViewModel>(
            this ViewModelObserver observer,
            CollectionViewModel<TItemModel, TItemViewModel> collection,
            Func<TItemViewModel, IObserverNode> itemObserverGenerator
            )
            where TItemViewModel : WrapperViewModel<TItemModel>
        {
            observer.RegisterCollection(collection, itemObserverGenerator);
            return observer;
        }

        public static ViewModelObserver ObservingViewModel(this ViewModelObserver observer, IObserverNode innerObserver)
        {
            observer.RegisterViewModel(innerObserver);
            return observer;
        }
    }
}
