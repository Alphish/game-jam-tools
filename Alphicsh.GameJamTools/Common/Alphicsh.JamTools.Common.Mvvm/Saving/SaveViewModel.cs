using Alphicsh.JamTools.Common.IO.Saving;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.Modals;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.Mvvm.Saving
{
    public class SaveViewModel<TModel, TViewModel> : BaseViewModel, ISaveViewModel
        where TViewModel : WrapperViewModel<TModel>
    {
        public ISaveModel<TModel> SaveModel { get; }
        
        private SaveDataObserver<TViewModel> DataObserver { get; }
        private TViewModel? ViewModel { get; set; }

        public SaveViewModel(ISaveModel<TModel> saveModel, SaveDataObserver<TViewModel> dataObserver)
        {
            SaveModel = saveModel;
            DataObserver = dataObserver;
            DataObserver.SetViewModel(this);
            ViewModel = null;

            IsModifiedProperty = NotifiableProperty.Create(this, nameof(IsModified));
            SaveCommand = ConditionalCommand.From(CanSave, Save);
        }

        public NotifiableProperty IsModifiedProperty { get; }
        public bool IsModified => SaveModel.IsModified;

        public IConditionalCommand SaveCommand { get; }
        private bool CanSave() => ViewModel != null;
        public void Save()
        {
            SaveModel.Save(ViewModel!.Model);
            IsModifiedProperty.RaisePropertyChanged();
        }

        public bool TrySaveOnClose()
        {
            if (!CanSave() || !IsModified)
                return true;

            return SaveOnCloseViewModel.ShowModal(this);
        }

        // ----------------
        // Model management
        // ----------------

        public void TrackViewModel(TViewModel viewModel)
        {
            ViewModel = viewModel;
            DataObserver.ObserveViewModel(viewModel);
            SaveModel.LoadSavedModel(viewModel.Model);
            SaveModel.UpdateCurrentModel(viewModel.Model);

            IsModifiedProperty.RaisePropertyChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void DropViewModel()
        {
            ViewModel = null;
            SaveModel.DropModel();

            IsModifiedProperty.RaisePropertyChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        public void UpdateCurrentData()
        {
            var wasModified = SaveModel.IsModified;
            SaveModel.UpdateCurrentModel(ViewModel!.Model);

            if (wasModified != IsModified)
                IsModifiedProperty.RaisePropertyChanged();
        }
    }
}
