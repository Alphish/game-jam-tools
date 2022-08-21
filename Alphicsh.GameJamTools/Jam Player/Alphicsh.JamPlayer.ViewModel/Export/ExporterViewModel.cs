using System.Windows;
using System.Windows.Input;
using Alphicsh.JamPlayer.Model.Export;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Export
{
    public class ExporterViewModel : WrapperViewModel<Exporter>
    {
        public ExporterViewModel(Exporter model) : base(model)
        {
            Options = new ExportOptionsViewModel(model.Options);

            ExportedTextProperty = WrapperProperty.Create(
                this, nameof(ExportedText), vm => vm.Model.ExportedText, (vm, value) => vm.Model.ExportedText = value
                );

            GenerateTextCommand = SimpleCommand.From(GenerateText);
            CopyTextCommand = SimpleCommand.From(CopyText);
        }

        public ExportOptionsViewModel Options { get; }

        public WrapperProperty<ExporterViewModel, string> ExportedTextProperty { get; }
        public string ExportedText { get => ExportedTextProperty.Value; set => ExportedTextProperty.Value = value; }

        public ICommand GenerateTextCommand { get; }
        private void GenerateText()
        {
            Model.GenerateText();
            ExportedTextProperty.RaisePropertyChanged();
        }

        public ICommand CopyTextCommand { get; }
        private void CopyText()
        {
            Clipboard.SetText(Model.ExportedText);
        }
    }
}
