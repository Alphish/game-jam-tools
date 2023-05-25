using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Alphicsh.EntryPackager.Model.Entry.Export;
using Alphicsh.EntryPackager.ViewModel.Entry.Files;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Observation;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Export
{
    public class JamEntryChecklistViewModel : WrapperViewModel<JamEntryChecklist>
    {
        public JamEntryChecklistViewModel(JamEntryChecklist model) : base(model)
        {
            // Statuses setup

            InnerStatuses = new List<ChecklistItemViewModel>();
            CreateStatusItem(GetTitleDescription, () => Model.TitleStatus, ObserveTitle);
            CreateStatusItem(GetAuthorsDescription, () => Model.AuthorsStatus, ObserveAuthors);
            CreateStatusItem(GetLaunchersDescription, () => Model.LaunchersStatus, ObserveLaunchers);
            CreateStatusItem(GetReadmeDescription, () => Model.ReadmeStatus, ObserveReadme);
            CreateStatusItem(GetAfterwordDescription, () => Model.AfterwordStatus, ObserveAfterword);
            CreateStatusItem(GetThumbnailsDescription, () => Model.ThumbnailsStatus, ObserveThumbnails);

            // Reminders setup

            InnerReminders = new List<ReminderItemViewModel>();
            CreateReminder(
                vm => vm.Checklist.IsCopyrightConfirmed,
                "All code and assets in the game were either made by the team, used with permission or according to the license"
                );
            CreateReminder(
                vm => vm.Checklist.AreCreditsConfirmed,
                "Any code or assets made before the Jam and/or not made by the team was properly credited in the Readme file or the game itself"
                );
            CreateReminder(
                vm => vm.Checklist.AreInstructionsConfirmed,
                "All necessary controls and instructions are in the game itself or a Readme file (mark Readme as required, if no in-game instructions are available)"
                );
            CreateReminder(
                vm => vm.Checklist.IsCompressedAudioConfirmed,
                "No music assets was accidentally left on \"Uncompressed - Not Streamed\" setting, making the game files unnecessarily large (short sound effects are fine uncompressed)"
                );
        }

        // --------
        // Statuses
        // --------

        private List<ChecklistItemViewModel> InnerStatuses { get; }
        public IReadOnlyCollection<ChecklistItemViewModel> Statuses => InnerStatuses;

        private void CreateStatusItem(
            Func<string> descriptionSelector,
            Func<ChecklistStatus> statusSelector,
            Func<ChecklistItemObserver, IObserverNode> observerNodeGenerator
            )
        {
            var item = new ChecklistItemViewModel(descriptionSelector, statusSelector, observerNodeGenerator);
            InnerStatuses.Add(item);
        }

        // Title

        private string GetTitleDescription()
        {
            if (Model.HasTitle)
                return "Entry has a title defined";
            else
                return "The title is required, but no title was given";
        }

        private IObserverNode ObserveTitle(ChecklistItemObserver observer)
        {
            var entryViewModel = EntryPackagerViewModel.Current.Entry!;
            return observer.CreateViewModelObserver()
                .ObservingProperty(entryViewModel.TitleProperty);
        }

        // Authors

        private string GetAuthorsDescription()
        {
            if (Model.HasAuthors)
                return "Entry has authors defined";
            else
                return "Authors are required, but no author was given";
        }

        private IObserverNode ObserveAuthors(ChecklistItemObserver observer)
        {
            var teamViewModel = EntryPackagerViewModel.Current.Entry!.Team;
            return observer.CreateViewModelObserver()
                .ObservingCollection(teamViewModel.Authors, author => observer.CreateViewModelObserver());
        }

        // Launchers

        private string GetLaunchersDescription()
        {
            if (!Model.HasLaunchers)
                return "Launchers are required, but no launcher was defined";
            else if (!Model.AllLaunchersAreValid)
                return "At least one of the given launchers is invalid";
            else
                return "Entry has launchers defined";
        }

        private IObserverNode ObserveLaunchers(ChecklistItemObserver observer)
        {
            var filesViewModel = EntryPackagerViewModel.Current.Entry!.Files;
            return observer.CreateViewModelObserver()
                .ObservingCollection(filesViewModel.Launchers, launcher => ObserveSingleLauncher(observer, launcher));
        }

        private IObserverNode ObserveSingleLauncher(ChecklistItemObserver observer, JamLauncherEditableViewModel launcherViewModel)
        {
            return observer.CreateViewModelObserver()
                .ObservingProperty(launcherViewModel.NameProperty)
                .ObservingProperty(launcherViewModel.TypeProperty)
                .ObservingProperty(launcherViewModel.LocationProperty);
        }

        // Readme

        private string GetReadmeDescription()
        {
            if (!Model.HasReadme)
                return "No Readme file was given";
            else if (!Model.IsReadmeValid)
                return "The given Readme file has an invalid location";
            else if (!Model.IsReadmeRequired)
                return "Entry has a Readme file";
            else
                return "Entry has a required Readme file";
        }

        private IObserverNode ObserveReadme(ChecklistItemObserver observer)
        {
            var readmeViewModel = EntryPackagerViewModel.Current.Entry!.Files.Readme;
            return observer.CreateViewModelObserver()
                .ObservingProperty(readmeViewModel.LocationProperty)
                .ObservingProperty(readmeViewModel.IsRequiredProperty);
        }

        // Afterword

        private string GetAfterwordDescription()
        {
            if (!Model.HasAfterword)
                return "No Afterword file was given";
            else if (!Model.IsAfterwordValid)
                return "The given Afterword file has an invalid location";
            else
                return "Entry has an Afterword file";
        }

        private IObserverNode ObserveAfterword(ChecklistItemObserver observer)
        {
            var afterwordViewModel = EntryPackagerViewModel.Current.Entry!.Files.Afterword;
            return observer.CreateViewModelObserver()
                .ObservingProperty(afterwordViewModel.LocationProperty);
        }

        // Thumbnails

        private string GetThumbnailsDescription()
        {
            if (!Model.HasThumbnail)
                return "No thumbnails were given";
            else if (!Model.HasBothThumbnails)
                return "Entry has a thumbnail";
            else
                return "Entry has both big and small thumbnails";
        }

        private IObserverNode ObserveThumbnails(ChecklistItemObserver observer)
        {
            var thumbnailsViewModel = EntryPackagerViewModel.Current.Entry!.Files.Thumbnails;
            return observer.CreateViewModelObserver()
                .ObservingProperty(thumbnailsViewModel.ThumbnailLocationProperty)
                .ObservingProperty(thumbnailsViewModel.ThumbnailSmallProperty);
        }

        // ---------
        // Reminders
        // ---------

        private List<ReminderItemViewModel> InnerReminders { get; }
        public IReadOnlyCollection<ReminderItemViewModel> Reminders => InnerReminders;

        private void CreateReminder(
            Expression<Func<ReminderItemViewModel, bool>> propertyExpression,
            string description
            )
        {
            var item = new ReminderItemViewModel(Model, propertyExpression, description);
            InnerReminders.Add(item);
        }
    }
}
