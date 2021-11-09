using System.ComponentModel;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public abstract class WrapperViewModel<TModel> : BaseViewModel
    {
        protected internal TModel Model { get; }

        protected WrapperViewModel(TModel model)
        {
            Model = model;
        }
    }
}
