using System;
using System.Linq.Expressions;
using Alphicsh.EntryPackager.Model.Entry.Export;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Export
{
    public class ReminderItemViewModel : BaseViewModel
    {
        public JamEntryChecklist Checklist { get; }

        public ReminderItemViewModel(
            JamEntryChecklist checklist,
            Expression<Func<ReminderItemViewModel, bool>> propertyExpression,
            string description
            )
        {
            Checklist = checklist;
            IsCheckedProperty = WrapperProperty.ForMember(this, propertyExpression);
            Description = description;
        }

        public WrapperProperty<ReminderItemViewModel, bool> IsCheckedProperty { get; }
        public bool IsChecked {  get => IsCheckedProperty.Value; set => IsCheckedProperty.Value = value; }

        public string Description { get; }
    }
}
