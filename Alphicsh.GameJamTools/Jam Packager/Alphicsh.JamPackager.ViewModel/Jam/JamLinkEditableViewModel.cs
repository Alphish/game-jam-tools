using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPackager.ViewModel.Jam
{
    public class JamLinkEditableViewModel : WrapperViewModel<JamLinkEditable>
    {
        public static CollectionViewModelStub<JamLinkEditable, JamLinkEditableViewModel> CollectionStub
            => CollectionViewModelStub.Create((JamLinkEditable model) => new JamLinkEditableViewModel(model));

        public JamLinkEditableViewModel(JamLinkEditable model) : base(model)
        {
            TitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.Title);
            UrlProperty = WrapperProperty.ForMember(this, vm => vm.Model.Url);
        }

        public WrapperProperty<JamLinkEditableViewModel, string> TitleProperty { get; }
        public string Title { get => TitleProperty.Value; set => TitleProperty.Value = value; }

        public WrapperProperty<JamLinkEditableViewModel, string> UrlProperty { get; }
        public string Url { get => UrlProperty.Value; set => UrlProperty.Value = value; }
    }
}
