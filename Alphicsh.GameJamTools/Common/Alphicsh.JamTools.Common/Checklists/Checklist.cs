using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTools.Common.Checklists
{
    public abstract class Checklist
    {
        public IReadOnlyCollection<CheckResult> Results { get; protected set; }
        public IReadOnlyCollection<CheckConfirmation> Confirmations { get; }

        public Checklist()
        {
            Results = GetResults().ToList();
            Confirmations = GetConfirmations().ToList();
        }

        public void Recheck()
        {
            Results = GetResults().ToList();
        }

        protected abstract IEnumerable<CheckResult> GetResults();
        protected abstract IEnumerable<CheckConfirmation> GetConfirmations();

        public bool IsReady =>
            Results.All(result => result.Status != ChecklistStatus.Invalid) &&
            Confirmations.All(confirmation => confirmation.IsConfirmed || !confirmation.IsMandatory);
    }
}
