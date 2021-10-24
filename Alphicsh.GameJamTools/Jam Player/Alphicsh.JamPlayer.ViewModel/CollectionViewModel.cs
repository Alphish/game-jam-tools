using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Alphicsh.JamPlayer.ViewModel
{
    public class CollectionViewModel<TModel, TViewModel> : IList<TViewModel>
        where TViewModel : BaseViewModel<TModel>
    {
        protected CollectionViewModelStub<TModel, TViewModel> Stub { get; }
        protected Func<TModel, TViewModel> ViewModelMapping => Stub.ViewModelMapping;

        protected IList<TModel> Models { get; }
        protected IList<TViewModel> ViewModels { get; }

        // --------
        // Creation
        // --------

        public CollectionViewModel(IList<TModel> modelEntries, CollectionViewModelStub<TModel, TViewModel> stub)
        {
            Stub = stub;

            Models = modelEntries;
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
            Models.Add(item.Model);
            ViewModels.Add(item);
        }

        public void CopyTo(TViewModel[] array, int arrayIndex)
        {
            ViewModels.CopyTo(array, arrayIndex);
        }

        public bool Remove(TViewModel item)
        {
            Models.Remove(item.Model);
            return ViewModels.Remove(item);
        }

        public void Clear()
        {
            Models.Clear();
            ViewModels.Clear();
        }

        // --------------------
        // IList implementation
        // --------------------

        public TViewModel this[int index]
        {
            get => ViewModels[index];
            set
            {
                Models[index] = value.Model;
                ViewModels[index] = value;
            }
        }

        public int IndexOf(TViewModel item)
        {
            return ViewModels.IndexOf(item);
        }

        public void Insert(int index, TViewModel item)
        {
            Models.Insert(index, item.Model);
            ViewModels.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Models.RemoveAt(index);
            ViewModels.RemoveAt(index);
        }
    }

    // ---------------
    // Static creation
    // ---------------

    public static class CollectionViewModel
    {
        public static CollectionViewModel<TModel, TViewModel> Create<TModel, TViewModel>(
            IEnumerable<TModel> modelEntries,
            CollectionViewModelStub<TModel, TViewModel> stub
            )
            where TViewModel : BaseViewModel<TModel>
        {
            return new CollectionViewModel<TModel, TViewModel>(modelEntries.ToList(), stub);
        }
    }
}
