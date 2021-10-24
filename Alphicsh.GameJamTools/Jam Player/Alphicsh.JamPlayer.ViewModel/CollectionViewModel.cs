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

        protected bool IsImmutable { get; }

        protected IList<TModel> Models { get; }
        protected IList<TViewModel> ViewModels { get; }

        // --------
        // Creation
        // --------

        public CollectionViewModel(IList<TModel> modelEntries, CollectionViewModelStub<TModel, TViewModel> stub, bool isImmutable)
        {
            Stub = stub;
            IsImmutable = isImmutable;

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

        public bool IsReadOnly => IsImmutable;

        public bool Contains(TViewModel item)
        {
            return ViewModels.Contains(item);
        }

        public void Add(TViewModel item)
        {
            EnsureCanWrite();
            Models.Add(item.Model);
            ViewModels.Add(item);
        }

        public void CopyTo(TViewModel[] array, int arrayIndex)
        {
            ViewModels.CopyTo(array, arrayIndex);
        }

        public bool Remove(TViewModel item)
        {
            EnsureCanWrite();
            Models.Remove(item.Model);
            return ViewModels.Remove(item);
        }

        public void Clear()
        {
            EnsureCanWrite();
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
                EnsureCanWrite();
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
            EnsureCanWrite();
            Models.Insert(index, item.Model);
            ViewModels.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            EnsureCanWrite();
            Models.RemoveAt(index);
            ViewModels.RemoveAt(index);
        }

        // -------------
        // Miscellaneous
        // -------------

        private void EnsureCanWrite()
        {
            if (IsReadOnly)
                throw new NotSupportedException("The given collection is read-only.");
        }
    }

    // ---------------
    // Static creation
    // ---------------

    public static class CollectionViewModel
    {
        public static CollectionViewModel<TModel, TViewModel> CreateImmutable<TModel, TViewModel>(
            IEnumerable<TModel> modelEntries,
            CollectionViewModelStub<TModel, TViewModel> stub
            )
            where TViewModel : BaseViewModel<TModel>
        {
            return new CollectionViewModel<TModel, TViewModel>(modelEntries.ToList(), stub, isImmutable: true);
        }

        public static CollectionViewModel<TModel, TViewModel> CreateMutable<TModel, TViewModel>(
            IList<TModel> modelEntries,
            CollectionViewModelStub<TModel, TViewModel> stub
            )
            where TViewModel : BaseViewModel<TModel>
        {
            return new CollectionViewModel<TModel, TViewModel>(modelEntries, stub, isImmutable: true);
        }
    }
}
