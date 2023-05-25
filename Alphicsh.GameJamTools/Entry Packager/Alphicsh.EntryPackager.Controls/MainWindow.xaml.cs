using System.ComponentModel;
using Alphicsh.EntryPackager.ViewModel;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.EntryPackager.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AppWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!SaveOnCloseViewModel.IsPromptShown)
            {
                var saveOnCloseResult = EntryPackagerViewModel.Current.SaveSystem.TrySaveOnClose();
                if (!saveOnCloseResult)
                    e.Cancel = true;
            }

            base.OnClosing(e);
        }
    }
}
