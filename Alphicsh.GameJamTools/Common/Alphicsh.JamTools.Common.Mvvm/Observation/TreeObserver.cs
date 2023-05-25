using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.Mvvm.Observation
{
    public abstract class TreeObserver : IObserverNode
    {
        public ICollection<IObserverNode> InnerObservers { get; } = new List<IObserverNode>();

        // -------------
        // Tree creation
        // -------------

        public ViewModelObserver CreateViewModelObserver()
        {
            return new ViewModelObserver(this);
        }

        // --------------------
        // Observers management
        // --------------------

        protected void AddObserver(IObserverNode observer)
        {
            InnerObservers.Add(observer);
        }

        protected void ClearObservers()
        {
            InnerObservers.Clear();
        }

        public void Observe()
        {
            foreach (var observer in InnerObservers)
            {
                observer.Observe();
            }
        }

        public void Unobserve()
        {
            foreach (var observer in InnerObservers)
            {
                observer.Unobserve();
            }
        }

        // --------------------
        // Execution management
        // --------------------

        private int ExecutionCounter { get; set; }
        public async void RecordObservation()
        {
            var counter = ++ExecutionCounter;
            await Task.Delay(50);
            if (counter != ExecutionCounter)
                return;

            OnObservation();
        }

        protected abstract void OnObservation();
    }
}
