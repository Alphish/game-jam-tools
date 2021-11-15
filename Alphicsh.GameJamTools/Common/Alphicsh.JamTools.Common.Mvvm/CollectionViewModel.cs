using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public class CollectionViewModel<TModel, TViewModel> : BaseViewModel, IList<TViewModel>, INotifyCollectionChanged
        where TViewModel : WrapperViewModel<TModel>
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
            ViewModels = new List<TViewModel>();
            SynchronizeWithModels();
        }

        // ----------------
        // Handling changes
        // ----------------

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public void SynchronizeWithModels()
        {
            var viewModels = Models.Select(ViewModelMapping);
            ViewModels.Clear();
            foreach (var viewModel in viewModels)
                ViewModels.Add(viewModel);

            CompleteChanges();
        }

        protected virtual void ApplyChanges()
        {
            // doing whichever stuff needs to be done just after collection changes
            // but just before the collection changed event is raised
        }

        public void CompleteChanges()
        {
            ApplyChanges();

            // for convenience sake, and also for the sake of firing only one event per set of changes
            // I'm just going to assume every change is a Reset change (cue dramatic SFX)
            // if it becomes a performance problem, *then* I'll think of implementing more fine-grained changes
            // (does WPF even handle the fine-grained changes separately? I'm pretty sure a few years ago it didn't...)
            CollectionChanged?.Invoke(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
                );
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
            where TViewModel : WrapperViewModel<TModel>
        {
            return new CollectionViewModel<TModel, TViewModel>(modelEntries.ToList(), stub, isImmutable: true);
        }

        public static CollectionViewModel<TModel, TViewModel> CreateMutable<TModel, TViewModel>(
            IList<TModel> modelEntries,
            CollectionViewModelStub<TModel, TViewModel> stub
            )
            where TViewModel : WrapperViewModel<TModel>
        {
            return new CollectionViewModel<TModel, TViewModel>(modelEntries, stub, isImmutable: true);
        }
    }
}
