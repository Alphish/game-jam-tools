using System;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public class AppViewModel<TModel> : WrapperViewModel<TModel>, IAppViewModel
    {
        protected AppViewModel(TModel model) : base(model)
        {
            if (AppViewModel.Current != null)
                throw new InvalidOperationException("AppViewModel should be created only once.");

            AppViewModel.Current = this;

            HasOverlayProperty = MutableProperty.Create(this, nameof(HasOverlay), false);
        }

        public MutableProperty<bool> HasOverlayProperty { get; }
        public bool HasOverlay { get => HasOverlayProperty.Value; set => HasOverlayProperty.Value = value; }
    }

    public static class AppViewModel
    {
        public static IAppViewModel Current { get; internal set; } = default!;
    }
}
