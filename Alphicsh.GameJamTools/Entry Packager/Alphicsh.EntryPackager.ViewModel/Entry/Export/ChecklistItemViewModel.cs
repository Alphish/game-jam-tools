using System;
using System.Windows.Media;
using Alphicsh.EntryPackager.Model.Entry.Export;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;
using Alphicsh.JamTools.Common.Mvvm.Observation;
using Alphicsh.JamTools.Common.Theming;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Export
{
    public class ChecklistItemViewModel : BaseViewModel
    {
        private Func<string> DescriptionSelector { get; }
        private Func<ChecklistStatus> StatusSelector { get; }
        private TreeObserver TreeObserver { get; }

        public ChecklistItemViewModel(
            Func<string> descriptionSelector,
            Func<ChecklistStatus> statusSelector,
            Func<ChecklistItemObserver, IObserverNode> observerNodeGenerator
            )
        {
            DescriptionSelector = descriptionSelector;
            StatusSelector = statusSelector;
            TreeObserver = new ChecklistItemObserver(this, observerNodeGenerator);

            DescriptionProperty = NotifiableProperty.Create(this, nameof(Description));
            StatusIconProperty = NotifiableProperty.Create(this, nameof(StatusIcon));
            StatusBrushProperty = NotifiableProperty.Create(this, nameof(StatusBrush));
            Refresh();
        }

        // ----------------
        // Refreshing state
        // ----------------

        internal void Refresh()
        {
            Description = DescriptionSelector();
            Status = StatusSelector();
            StatusIcon = GetSourceForStatus();
            StatusBrush = GetBrushForStatus();

            DescriptionProperty.RaisePropertyChanged();
            StatusIconProperty.RaisePropertyChanged();
            StatusBrushProperty.RaisePropertyChanged();
        }

        public NotifiableProperty DescriptionProperty { get; private set; }
        public string Description { get; private set; } = default!;

        public ChecklistStatus Status { get; private set; } = default!;

        public NotifiableProperty StatusIconProperty { get; }
        public ImageSource StatusIcon { get; private set; } = default!;

        public NotifiableProperty StatusBrushProperty { get; }
        public Brush StatusBrush { get; private set; } = default!;

        private ImageSource GetSourceForStatus()
        {
            switch (Status)
            {
                case ChecklistStatus.Present:
                    return ThemeManager.Current.IconCheckSource;
                case ChecklistStatus.Invalid:
                    return ThemeManager.Current.IconCrossSource;
                default:
                    return ThemeManager.Current.IconNilSource;
            }
        }

        private Brush GetBrushForStatus()
        {
            switch (Status)
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
