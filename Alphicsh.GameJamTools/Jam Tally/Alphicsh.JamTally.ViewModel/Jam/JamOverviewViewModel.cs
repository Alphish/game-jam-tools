﻿using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTally.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;

namespace Alphicsh.JamTally.ViewModel.Jam
{
    public class JamOverviewViewModel : WrapperViewModel<JamOverview>
    {
        public JamOverviewViewModel(JamOverview model) : base(model)
        {
            AvailableAlignments = MakeAvailableAlignments();
            AvailableEntries = MakeAvailableEntries();
        }

        // ----------
        // Alignments
        // ----------

        public bool HasAlignment => Model.Alignments != null;
        public IReadOnlyCollection<JamAlignmentOptionViewModel> AvailableAlignments { get; }

        private IReadOnlyCollection<JamAlignmentOptionViewModel> MakeAvailableAlignments()
        {
            if (Model.Alignments == null)
                return new List<JamAlignmentOptionViewModel>();

            var result = Model.Alignments.GetAvailableOptions()
                .Select(option => new JamAlignmentOptionViewModel(option, option.Title))
                .ToList();

            result.Insert(0, new JamAlignmentOptionViewModel(option: null, Model.Alignments.NeitherTitle));
            return result;
        }

        // -------
        // Entries
        // -------

        public IReadOnlyCollection<JamEntryOptionViewModel> AvailableEntries { get; }
        private IReadOnlyCollection<JamEntryOptionViewModel> MakeAvailableEntries()
        {
            var result = Model.Entries
                .OrderBy(entry => entry.FullLine)
                .Select(entry => new JamEntryOptionViewModel(entry, entry.FullLine))
                .ToList();

            result.Insert(0, new JamEntryOptionViewModel(entry: null, "<no entry selected>"));
            return result;
        }
    }
}
