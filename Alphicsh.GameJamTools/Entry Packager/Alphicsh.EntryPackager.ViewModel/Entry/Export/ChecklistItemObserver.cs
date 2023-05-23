using System;
using Alphicsh.JamTools.Common.Mvvm.Observation;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Export
{
    public class ChecklistItemObserver : TreeObserver
    {
        private ChecklistItemViewModel ChecklistItem { get; }

        public ChecklistItemObserver(
            ChecklistItemViewModel checklistItem,
            Func<ChecklistItemObserver, IObserverNode> observerNodeGenerator
            )
        {
            ChecklistItem = checklistItem;
            AddObserver(observerNodeGenerator(this));
            Observe();
        }

        protected override void OnObservation()
        {
            ChecklistItem.Refresh();
        }
    }
}
