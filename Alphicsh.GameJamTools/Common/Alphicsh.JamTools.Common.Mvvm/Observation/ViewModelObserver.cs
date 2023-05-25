using System;
using System.Collections.Generic;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.Mvvm.Observation
{
    public class ViewModelObserver : IObserverNode
    {
        private TreeObserver MainObserver { get; }
        private ICollection<IObserverNode> InnerObservers { get; }

        public ViewModelObserver(TreeObserver mainObserver)
        {
            MainObserver = mainObserver;
            InnerObservers = new List<IObserverNode>();
        }

        // ---------------------
        // Registering observers
        // ---------------------

        public void RegisterProperty(INotifiableProperty property)
        {
            var observer = new PropertyObserver(MainObserver, property);
            InnerObservers.Add(observer);
        }

        public void RegisterCollection<TItemModel, TItemViewModel>(
            CollectionViewModel<TItemModel, TItemViewModel> collection,
            Func<TItemViewModel, IObserverNode> itemObserverGenerator
            )
            where TItemViewModel : WrapperViewModel<TItemModel>
        {
            var observer = new CollectionObserver<TItemModel, TItemViewModel>(MainObserver, collection, itemObserverGenerator);
            InnerObservers.Add(observer);
        }

        public void RegisterViewModel(IObserverNode viewModelObserver)
        {
            InnerObservers.Add(viewModelObserver);
        }

        // ---------------------
        // Managing observations
        // ---------------------

        public void Observe()
        {
            foreach (var observer in InnerObservers)
            {
                observer.Observe();
            }
        }

        public void Unobserve()
        {
            foreach (var observer in InnerObservers)
            {
                observer.Unobserve();
            }
        }
    }
}
