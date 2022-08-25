namespace Alphicsh.JamPlayer.Model.Export
{
    public sealed class Exporter
    {
        private ExportedTextGenerator Generator { get; }
        private AppModel AppModel { get; }

        public string ExportedText { get; set; } = string.Empty;
        public ExportOptions Options { get; } = new ExportOptions();

        public Exporter(AppModel appModel)
        {
            Generator = new ExportedTextGenerator();
            AppModel = appModel;
        }

        public void GenerateText()
        {
            ExportedText = Generator.GenerateExportedText(AppModel, Options);
        }
    }
}
