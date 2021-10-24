using System;

namespace Alphicsh.JamPlayer.ViewModel
{
    public class CollectionViewModelStub<TModel, TViewModel>
        where TViewModel : BaseViewModel<TModel>
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
            where TViewModel : BaseViewModel<TModel>
        {
            return new CollectionViewModelStub<TModel, TViewModel>(viewModelMapping);
        }
    }
}
