using System.ComponentModel;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public abstract class WrapperViewModel<TModel> : BaseViewModel
    {
        public TModel Model { get; }

        protected WrapperViewModel(TModel model)
        {
            Model = model;
        }
    }
}
