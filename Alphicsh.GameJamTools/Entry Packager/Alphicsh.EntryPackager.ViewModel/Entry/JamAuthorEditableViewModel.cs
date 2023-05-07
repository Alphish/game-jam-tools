using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class JamAuthorEditableViewModel : WrapperViewModel<JamAuthorEditable>
    {
        public static CollectionViewModelStub<JamAuthorEditable, JamAuthorEditableViewModel> CollectionStubFor(
            JamTeamEditableViewModel teamVm
            )
        {
            return CollectionViewModelStub.Create((JamAuthorEditable model) => new JamAuthorEditableViewModel(model, teamVm));
        }

        public JamAuthorEditableViewModel(JamAuthorEditable model, JamTeamEditableViewModel teamVm) : base(model)
        {
            TeamViewModel = teamVm;

            NameProperty = WrapperProperty.ForMember(this, vm => vm.Model.Name)
                .WithDependingProperty(TeamViewModel, nameof(TeamViewModel.AuthorsString));
            CommunityIdProperty = WrapperProperty.ForMember(this, vm => vm.Model.CommunityId);
            RoleProperty = WrapperProperty.ForMember(this, vm => vm.Model.Role);
        }

        private JamTeamEditableViewModel TeamViewModel { get; }

        public WrapperProperty<JamAuthorEditableViewModel, string> NameProperty { get; }
        public string Name { get => NameProperty.Value; set => NameProperty.Value = value; }

        public WrapperProperty<JamAuthorEditableViewModel, string?> CommunityIdProperty { get; }
        public string? CommunityId { get => CommunityIdProperty.Value; set => CommunityIdProperty.Value = value; }

        public WrapperProperty<JamAuthorEditableViewModel, string?> RoleProperty { get; }
        public string? Role { get => RoleProperty.Value; set => RoleProperty.Value = value; }
    }
}
