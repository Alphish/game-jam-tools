using System.Linq;

namespace Alphicsh.EntryPackager.Model.Entry.Export
{
    public class JamEntryChecklist
    {
        private JamEntryEditable EntryData { get; }

        public JamEntryChecklist(JamEntryEditable entryData)
        {
            EntryData = entryData;
        }

        // ---------
        // Checklist
        // ---------

        public bool HasTitle => !string.IsNullOrEmpty(EntryData.Title);
        public ChecklistStatus TitleStatus
            => HasTitle ? ChecklistStatus.Present : ChecklistStatus.Invalid;

        public bool HasAuthors => EntryData.Team.Authors.Any();
        public ChecklistStatus AuthorsStatus
            => HasAuthors ? ChecklistStatus.Present : ChecklistStatus.Invalid;

        public bool HasLaunchers => EntryData.Files.Launchers.Any();
        public bool AllLaunchersAreValid => EntryData.Files.Launchers.All(launcher => launcher.CanLaunch);
        public ChecklistStatus LaunchersStatus
            => HasLaunchers && AllLaunchersAreValid ? ChecklistStatus.Present : ChecklistStatus.Invalid;

        public bool HasReadme => !EntryData.Files.Readme.IsEmpty;
        public bool IsReadmeValid => !HasReadme || EntryData.Files.Readme.CanOpen;
        public bool IsReadmeRequired => HasReadme && EntryData.Files.Readme.IsRequired;
        public ChecklistStatus ReadmeStatus
            => !HasReadme ? ChecklistStatus.Absent : IsReadmeValid ? ChecklistStatus.Present : ChecklistStatus.Invalid;

        public bool HasAfterword => !EntryData.Files.Afterword.IsEmpty;
        public bool IsAfterwordValid => !HasAfterword || EntryData.Files.Afterword.CanOpen;
        public ChecklistStatus AfterwordStatus
            => !HasAfterword ? ChecklistStatus.Absent : IsAfterwordValid ? ChecklistStatus.Present : ChecklistStatus.Invalid;

        public bool HasThumbnail => !EntryData.Files.Thumbnails.IsEmpty;
        public bool HasBothThumbnails
            => EntryData.Files.Thumbnails.HasThumbnailLocation && EntryData.Files.Thumbnails.HasThumbnailSmallLocation;
        public ChecklistStatus ThumbnailsStatus
            => !HasThumbnail ? ChecklistStatus.Absent : ChecklistStatus.Present;

        public bool IsEntryReady()
        {
            return TitleStatus != ChecklistStatus.Invalid
                && AuthorsStatus != ChecklistStatus.Invalid
                && LaunchersStatus != ChecklistStatus.Invalid
                && ReadmeStatus != ChecklistStatus.Invalid
                && AfterwordStatus != ChecklistStatus.Invalid
                && ThumbnailsStatus != ChecklistStatus.Invalid;
        }
    }
}
