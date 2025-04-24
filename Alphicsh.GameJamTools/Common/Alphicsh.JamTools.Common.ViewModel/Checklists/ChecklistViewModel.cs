using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.Checklists;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.ViewModel.Checklists
{
    public class ChecklistViewModel : WrapperViewModel<Checklist>
    {
        public ChecklistViewModel(Checklist model) : base(model)
        {
            ResultsProperty = MutableProperty.Create(
                this,
                nameof(Results),
                model.Results.Select(checkResult => new CheckResultViewModel(checkResult)).ToList() as IReadOnlyCollection<CheckResultViewModel>
                );

            ConfirmationsProperty = MutableProperty.Create(
                this,
                nameof(Confirmations),
                model.Confirmations.Select(confirmation => new CheckConfirmationViewModel(confirmation)).ToList() as IReadOnlyCollection<CheckConfirmationViewModel>
                );

            IsReadyProperty = WrapperProperty.ForReadonlyMember(this, vm => vm.Model.IsReady);

            foreach (var confirmationVm in Confirmations)
            {
                confirmationVm.IsConfirmedProperty.AddDependingProperty(IsReadyProperty);
            }
        }

        public MutableProperty<IReadOnlyCollection<CheckResultViewModel>> ResultsProperty { get; }
        public IReadOnlyCollection<CheckResultViewModel> Results { get => ResultsProperty.Value; set => ResultsProperty.Value = value; }

        public MutableProperty<IReadOnlyCollection<CheckConfirmationViewModel>> ConfirmationsProperty { get; }
        public IReadOnlyCollection<CheckConfirmationViewModel> Confirmations { get => ConfirmationsProperty.Value; set => ConfirmationsProperty.Value = value; }

        public WrapperProperty<ChecklistViewModel, bool> IsReadyProperty { get; }
        public bool IsReady { get => IsReadyProperty.Value; }

        public void Recheck()
        {
            Model.Recheck();
            Results = Model.Results.Select(checkResult => new CheckResultViewModel(checkResult)).ToList();
            IsReadyProperty.RaisePropertyChanged();
        }
    }
}
