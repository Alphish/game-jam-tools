using Alphicsh.JamTools.Common.IO.Saving;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.Mvvm.Saving
{
    public class SaveViewModel<TModel, TViewModel> : BaseViewModel, ISaveViewModel
        where TViewModel : WrapperViewModel<TModel>
    {
        private ISaveModel<TModel> SaveModel { get; }
        private SaveDataObserver<TViewModel> DataObserver { get; }
        private TViewModel? ViewModel { get; set; }

        public SaveViewModel(ISaveModel<TModel> saveModel, SaveDataObserver<TViewModel> dataObserver)
        {
            SaveModel = saveModel;
            DataObserver = dataObserver;
            DataObserver.SetViewModel(this);
            ViewModel = null;

            HasChangesProperty = NotifiableProperty.Create(this, nameof(HasChanges));
            SaveCommand = ConditionalCommand.From(CanSave, Save);
        }

        public NotifiableProperty HasChangesProperty { get; }
        public bool HasChanges => SaveModel.HasChanges;

        public IConditionalCommand SaveCommand { get; }
        private bool CanSave() => ViewModel != null;
        private void Save()
        {
            SaveModel.Save(ViewModel!.Model);
            HasChangesProperty.RaisePropertyChanged();
        }

        // ----------------
        // Model management
        // ----------------

        public void TrackViewModel(TViewModel viewModel)
        {
            ViewModel = viewModel;
            DataObserver.ObserveViewModel(viewModel);
            SaveModel.AcceptModel(viewModel.Model);

            HasChangesProperty.RaisePropertyChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void DropViewModel()
        {
            ViewModel = null;
            SaveModel.DropModel();

            HasChangesProperty.RaisePropertyChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void UpdateCurrentData()
        {
            var hadChanges = SaveModel.HasChanges;
            SaveModel.ChangeModel(ViewModel!.Model);

            if (hadChanges != HasChanges)
                HasChangesProperty.RaisePropertyChanged();
        }
    }
}
