using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPackager.ViewModel.Jam
{
    public class JamAwardEditableViewModel : WrapperViewModel<JamAwardEditable>
    {
        public static CollectionViewModelStub<JamAwardEditable, JamAwardEditableViewModel> CollectionStub
            => CollectionViewModelStub.Create((JamAwardEditable model) => new JamAwardEditableViewModel(model));

        public JamAwardEditableViewModel(JamAwardEditable model) : base(model)
        {
            IdProperty = WrapperProperty.ForMember(this, vm => vm.Model.Id);
            NameProperty = WrapperProperty.ForMember(this, vm => vm.Model.Name);
            DescriptionProperty = WrapperProperty.ForMember(this, vm => vm.Model.Description);
        }

        public WrapperProperty<JamAwardEditableViewModel, string> IdProperty { get; }
        public string Id { get => IdProperty.Value; set => IdProperty.Value = value; }

        public WrapperProperty<JamAwardEditableViewModel, string> NameProperty { get; }
        public string Name { get => NameProperty.Value; set => NameProperty.Value = value; }

        public WrapperProperty<JamAwardEditableViewModel, string?> DescriptionProperty { get; }
        public string? Description { get => DescriptionProperty.Value; set => DescriptionProperty.Value = value; }
    }
}
