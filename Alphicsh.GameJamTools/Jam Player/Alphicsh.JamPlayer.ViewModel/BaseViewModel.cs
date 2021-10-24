namespace Alphicsh.JamPlayer.ViewModel
{
    public abstract class BaseViewModel<TModel>
    {
        protected TModel Model { get; }

        protected BaseViewModel(TModel model)
        {
            Model = model;
        }
    }
}
