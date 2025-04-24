using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamTools.Common.Checklists;

namespace Alphicsh.EntryPackager.Model.Entry.Export
{
    public class JamEntryChecklist : Checklist
    {
        private JamEntryEditable EntryData { get; }

        public JamEntryChecklist(JamEntryEditable entryData)
        {
            EntryData = entryData;
            Recheck();
        }

        protected override IEnumerable<CheckResult> GetResults()
        {
            if (EntryData == null)
                yield break;

            if (!string.IsNullOrEmpty(EntryData.Title))
                yield return new CheckResult { Status = ChecklistStatus.Present, Description = "Entry has a title defined" };
            else
                yield return new CheckResult { Status = ChecklistStatus.Invalid, Description = "The title is required, but no title was given" };

            if (EntryData.Team.Authors.Any())
                yield return new CheckResult { Status = ChecklistStatus.Present, Description = "Entry has authors defined" };
            else
                yield return new CheckResult { Status = ChecklistStatus.Invalid, Description = "Authors are required, but no author was given" };

            if (!EntryData.Files.Launchers.Any())
                yield return new CheckResult { Status = ChecklistStatus.Invalid, Description = "Launchers are required, but no launcher was defined" };
            else if (EntryData.Files.Launchers.Any(launcher => !launcher.CanLaunch))
                yield return new CheckResult { Status = ChecklistStatus.Invalid, Description = "At least one of the given launchers is invalid" };
            else
                yield return new CheckResult { Status = ChecklistStatus.Present, Description = "Entry has launchers defined" };

            if (EntryData.Files.Readme.IsEmpty)
                yield return new CheckResult { Status = ChecklistStatus.Absent, Description = "No Readme file was given" };
            else if (!EntryData.Files.Readme.CanOpen)
                yield return new CheckResult { Status = ChecklistStatus.Invalid, Description = "The given Readme file has an invalid location" };
            else if (!EntryData.Files.Readme.IsRequired)
                yield return new CheckResult { Status = ChecklistStatus.Present, Description = "Entry has a Readme file" };
            else
                yield return new CheckResult { Status = ChecklistStatus.Present, Description = "Entry has a required Readme file" };

            if (EntryData.Files.Afterword.IsEmpty)
                yield return new CheckResult { Status = ChecklistStatus.Absent, Description = "No Afterword file was given" };
            else if (EntryData.Files.Afterword.CanOpen)
                yield return new CheckResult { Status = ChecklistStatus.Present, Description = "The given Afterword file has an invalid location" };
            else
                yield return new CheckResult { Status = ChecklistStatus.Invalid, Description = "Entry has an Afterword file" };

            if (EntryData.Files.Thumbnails.IsEmpty)
                yield return new CheckResult { Status = ChecklistStatus.Absent, Description = "No thumbnails were given" };
            else if (EntryData.Files.Thumbnails.HasThumbnailLocation && EntryData.Files.Thumbnails.HasThumbnailSmallLocation)
                yield return new CheckResult { Status = ChecklistStatus.Present, Description = "Entry has a thumbnail" };
            else
                yield return new CheckResult { Status = ChecklistStatus.Present, Description = "Entry has both big and small thumbnails" };
        }

        protected override IEnumerable<CheckConfirmation> GetConfirmations()
        {
            yield return new CheckConfirmation { Description = "All code and assets in the game were either made by the team, used with permission or according to the license" };
            yield return new CheckConfirmation { Description = "Any code or assets made before the Jam and/or not made by the team was properly credited in the Readme file or the game itself" };
            yield return new CheckConfirmation { Description = "All necessary controls and instructions are in the game itself or a Readme file (mark Readme as required, if no in-game instructions are available)" };
            yield return new CheckConfirmation { Description = "No music assets was accidentally left on \"Uncompressed - Not Streamed\" setting, making the game files unnecessarily large (short sound effects are fine uncompressed)" };
        }
    }
}
