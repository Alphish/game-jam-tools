﻿using System.Windows.Input;
using System.Windows.Media;
using Alphicsh.JamPlayer.Model.Jam;
using Alphicsh.JamPlayer.ViewModel.Jam.Modals;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Jam
{
    public sealed class JamOverviewViewModel : WrapperViewModel<JamOverview>
    {
        public JamOverviewViewModel(JamOverview model)
            : base(model)
        {
            LogoPathProperty = ImageSourceProperty.CreateReadonly(this, nameof(Logo), vm => vm.Model.LogoPath);
            Entries = CollectionViewModel.CreateImmutable(model.Entries, JamEntryViewModel.CollectionStub);

            ConfirmResetDataCommand = SimpleCommand.From(ConfirmResetDataViewModel.ShowModal);
        }

        public string? Title => Model.Title ?? "The Game Jam";
        public string? Theme => Model.Theme;

        public ImageSourceProperty<JamOverviewViewModel> LogoPathProperty { get; }
        public ImageSource? Logo => LogoPathProperty.ImageSource;

        public CollectionViewModel<JamEntry, JamEntryViewModel> Entries { get; }

        public ICommand ConfirmResetDataCommand { get; }
    }
}
