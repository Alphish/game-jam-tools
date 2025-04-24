using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Compression;
using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.EntryPackager.Model.Entry.Export
{
    public class JamEntryExporter
    {
        private ZipCompressor Compressor { get; } = new ZipCompressor();
        private PackageNameFormatter NameFormatter { get; } = new PackageNameFormatter();

        public JamEntryEditable EntryData { get; }
        public ISaveModel<JamEntryEditable> SaveModel { get; }
        public JamEntryChecklist Checklist { get; }

        public JamEntryExporter(JamEntryEditable entryData, ISaveModel<JamEntryEditable> saveModel)
        {
            EntryData = entryData;
            SaveModel = saveModel;
            Checklist = new JamEntryChecklist(entryData);
        }

        public bool CanExport()
        {
            return Checklist.IsReady;
        }

        public void ExportTo(FilePath zipPath)
        {
            if (!Checklist.IsReady)
                return;

            SaveModel.Save(EntryData);
            Compressor.CompressDirectoryContentsTo(zipPath, EntryData.Files.DirectoryPath);
        }

        public string GetDefaultPackageName()
        {
            return NameFormatter.GetPackageName(EntryData.DisplayShortTitle, EntryData.Team.DisplayName);
        }
    }
}
