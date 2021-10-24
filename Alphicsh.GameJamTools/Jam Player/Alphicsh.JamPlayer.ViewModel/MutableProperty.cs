using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.ViewModel
{
    public class MutableProperty<TValue>
    {
        private IViewModel ViewModel { get; }
        private string PropertyName { get; }
        private TValue InnerValue { get; set; }

        private IReadOnlyCollection<string> DependingProperties { get; }

        public MutableProperty(IViewModel viewModel, string propertyName, TValue initialValue, IEnumerable<string> dependingProperties = null)
        {
            ViewModel = viewModel;
            PropertyName = propertyName;
            InnerValue = initialValue;

            DependingProperties = dependingProperties?.ToList() ?? new List<string>();
        }

        public TValue Value
        {
            get => InnerValue;
            set
            {
                if (object.Equals(InnerValue, value))
                    return;

                InnerValue = value;

                ViewModel.RaisePropertyChanged(PropertyName);
                foreach (var dependingProperty in DependingProperties)
                {
                    ViewModel.RaisePropertyChanged(dependingProperty);
                }
            }
        }
    }
}
