using System.Windows.Media;
using Alphicsh.JamTools.Common.Checklists;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Theming;

namespace Alphicsh.JamTools.Common.ViewModel.Checklists
{
    public class CheckResultViewModel : WrapperViewModel<CheckResult>
    {
        public CheckResultViewModel(CheckResult model) : base(model)
        {
            Description = Model.Description;
            Status = Model.Status;
            StatusIcon = GetIconSource(Model.Status);
            StatusBrush = GetIconBrush(Model.Status);
        }

        public string Description { get; }
        public ChecklistStatus Status { get; }
        public ImageSource StatusIcon { get; }
        public Brush StatusBrush { get; }

        private ImageSource GetIconSource(ChecklistStatus status)
        {
            switch (status)
            {
                case ChecklistStatus.Present:
                    return ThemeManager.Current.IconCheckSource;
                case ChecklistStatus.Invalid:
                    return ThemeManager.Current.IconCrossSource;
                default:
                    return ThemeManager.Current.IconNilSource;
            }
        }

        private Brush GetIconBrush(ChecklistStatus status)
        {
            switch (status)
            {
                case ChecklistStatus.Present:
                    return ThemeManager.Current.HighlightText;
                case ChecklistStatus.Invalid:
                    return ThemeManager.Current.ErrorText;
                default:
                    return ThemeManager.Current.ExtraDimText;
            }
        }
    }
}
