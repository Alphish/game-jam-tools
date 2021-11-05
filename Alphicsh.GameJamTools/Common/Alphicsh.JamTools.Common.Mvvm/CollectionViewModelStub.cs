using System;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public class CollectionViewModelStub<TModel, TViewModel>
        where TViewModel : WrapperViewModel<TModel>
    {
        public Func<TModel, TViewModel> ViewModelMapping { get; }

        public CollectionViewModelStub(Func<TModel, TViewModel> viewModelMapping)
        {
            ViewModelMapping = viewModelMapping;
        }
    }

    // ---------------
    // Static creation
    // ---------------

    public static class CollectionViewModelStub
    {
        public static CollectionViewModelStub<TModel, TViewModel> Create<TModel, TViewModel>(Func<TModel, TViewModel> viewModelMapping)
            where TViewModel : WrapperViewModel<TModel>
        {
            return new CollectionViewModelStub<TModel, TViewModel>(viewModelMapping);
        }
    }
}
