using System.Windows.Input;
using Alphicsh.JamPlayer.Model.Export;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Export
{
    public class ExportOptionsViewModel : WrapperViewModel<ExportOptions>
    {
        public ExportOptionsViewModel(ExportOptions model) : base(model)
        {
            ReviewsTitleProperty = WrapperProperty.Create(
                this, nameof(ReviewsTitle), vm => vm.Model.ReviewsTitle, (vm, value) => vm.Model.ReviewsTitle = value
                );
            ExportIncompleteRankingsProperty = WrapperProperty.Create(
                this, nameof(ExportIncompleteRankings),
                vm => vm.Model.ExportIncompleteRankings,
                (vm, value) => vm.Model.ExportIncompleteRankings = value
                );
            EntryCommentTemplateProperty = WrapperProperty.Create(
                this, nameof(EntryCommentTemplate), vm => vm.Model.EntryCommentTemplate, (vm, value) => vm.Model.EntryCommentTemplate = value
                );

            RestoreDefaultsCommand = SimpleCommand.From(RestoreDefaults);
        }

        public WrapperProperty<ExportOptionsViewModel, string> ReviewsTitleProperty { get; }
        public string ReviewsTitle { get => ReviewsTitleProperty.Value; set => ReviewsTitleProperty.Value = value; }
        public WrapperProperty<ExportOptionsViewModel, bool> ExportIncompleteRankingsProperty { get; }
        public bool ExportIncompleteRankings { get => ExportIncompleteRankingsProperty.Value; set => ExportIncompleteRankingsProperty.Value = value; }
        public WrapperProperty<ExportOptionsViewModel, string> EntryCommentTemplateProperty { get; }
        public string EntryCommentTemplate { get => EntryCommentTemplateProperty.Value; set => EntryCommentTemplateProperty.Value = value; }

        public ICommand RestoreDefaultsCommand { get; }
        private void RestoreDefaults()
        {
            Model.RestoreDefaults();
            ReviewsTitleProperty.RaisePropertyChanged();
            ExportIncompleteRankingsProperty.RaisePropertyChanged();
            EntryCommentTemplateProperty.RaisePropertyChanged();
        }
    }
}
