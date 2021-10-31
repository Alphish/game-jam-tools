namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public class NotifiableProperty : INotifiableProperty
    {
        protected IViewModel ViewModel { get; }
        protected string PropertyName { get; }

        public NotifiableProperty(IViewModel viewModel, string propertyName)
        {
            ViewModel = viewModel;
            PropertyName = propertyName;
        }

        public static NotifiableProperty Create(IViewModel viewModel, string propertyName)
        {
            return new NotifiableProperty(viewModel, propertyName);
        }

        public void RaisePropertyChanged()
        {
            ViewModel.RaisePropertyChanged(PropertyName);
        }
    }
}
