using Alphicsh.JamTools.Common.Checklists;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.ViewModel.Checklists
{
    public class CheckConfirmationViewModel : WrapperViewModel<CheckConfirmation>
    {
        public CheckConfirmationViewModel(CheckConfirmation model) : base(model)
        {
            Description = model.Description;

            IsConfirmedProperty = WrapperProperty.ForMember(this, vm => vm.Model.IsConfirmed);
        }

        public string Description { get; }

        public WrapperProperty<CheckConfirmationViewModel, bool> IsConfirmedProperty { get; }
        public bool IsConfirmed { get => IsConfirmedProperty.Value; set => IsConfirmedProperty.Value = value; }
    }
}
