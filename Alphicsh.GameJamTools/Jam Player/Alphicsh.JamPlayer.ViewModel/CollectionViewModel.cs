using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Alphicsh.JamPlayer.ViewModel
{
    public class CollectionViewModel<TModel, TViewModel> : IList<TViewModel>
        where TViewModel : BaseViewModel<TModel>
    {
        protected Func<TModel, TViewModel> ViewModelMapping { get; }
        protected IList<TViewModel> ViewModels { get; }

        public CollectionViewModel(IEnumerable<TModel> modelEntries, Func<TModel, TViewModel> viewModelMapping)
        {
            ViewModelMapping = viewModelMapping;
            ViewModels = modelEntries.Select(ViewModelMapping).ToList();
        }

        // --------------------------
        // IEnumerable implementation
        // --------------------------

        public IEnumerator<TViewModel> GetEnumerator()
        {
            return ViewModels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)ViewModels).GetEnumerator();
        }

        // --------------------------
        // ICollection implementation
        // --------------------------

        public int Count => ViewModels.Count;

        public bool IsReadOnly => ViewModels.IsReadOnly;

        public bool Contains(TViewModel item)
        {
            return ViewModels.Contains(item);
        }

        public void Add(TViewModel item)
        {
            ViewModels.Add(item);
        }

        public void CopyTo(TViewModel[] array, int arrayIndex)
        {
            ViewModels.CopyTo(array, arrayIndex);
        }

        public bool Remove(TViewModel item)
        {
            return ViewModels.Remove(item);
        }

        public void Clear()
        {
            ViewModels.Clear();
        }

        // --------------------
        // IList implementation
        // --------------------

        public TViewModel this[int index]
        {
            get => ViewModels[index];
            set => ViewModels[index] = value;
        }

        public int IndexOf(TViewModel item)
        {
            return ViewModels.IndexOf(item);
        }

        public void Insert(int index, TViewModel item)
        {
            ViewModels.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ViewModels.RemoveAt(index);
        }
    }
}
