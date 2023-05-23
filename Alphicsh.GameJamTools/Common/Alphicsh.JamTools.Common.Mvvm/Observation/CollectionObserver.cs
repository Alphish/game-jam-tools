using System;
using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.Mvvm.Observation
{
    public class CollectionObserver<TItemModel, TItemViewModel> : IObserverNode
        where TItemViewModel : WrapperViewModel<TItemModel>
    {
        private TreeObserver MainObserver { get; }
        private CollectionViewModel<TItemModel, TItemViewModel> Collection { get; }
        private Func<TItemViewModel, IObserverNode> ItemObserverGenerator { get; }

        private ICollection<IObserverNode> InnerObservers { get; }
        
        public CollectionObserver(
            TreeObserver mainObserver,
            CollectionViewModel<TItemModel, TItemViewModel> collection,
            Func<TItemViewModel, IObserverNode> itemObserverGenerator
            )
        {
            MainObserver = mainObserver;
            Collection = collection;
            ItemObserverGenerator = itemObserverGenerator;
            InnerObservers = new List<IObserverNode>();
        }

        public void Observe()
        {
            Collection.CollectionChanged += OnCollectionChanged;
            GenerateObservers();
        }

        public void Unobserve()
        {
            ClearObservers();
            Collection.CollectionChanged -= OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, EventArgs e)
        {
            ClearObservers();
            GenerateObservers();
            MainObserver.RecordObservation();
        }

        private void GenerateObservers()
        {
            foreach (var item in Collection)
            {
                var itemObserver = ItemObserverGenerator(item);
                InnerObservers.Add(itemObserver);
                itemObserver.Observe();
            }
        }

        private void ClearObservers()
        {
            foreach (var observer in InnerObservers)
            {
                observer.Unobserve();
            }
            InnerObservers.Clear();
        }
    }
}
