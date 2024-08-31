using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage
{
    public class FileDataManager
    {
        // -------
        // Loading
        // -------

        public async Task<FileData?> LoadFile(FilePath path)
        {
            if (!path.HasFile())
                return null;

            var content = await File.ReadAllTextAsync(path.Value, Encoding.UTF8);
            return new FileData(path, content);
        }

        public async Task<FileBatch> LoadBatch(FileData coreFile, IEnumerable<FilePath> auxiliaryPaths)
        {
            var auxiliaryFilesTasks = auxiliaryPaths.Select(path => LoadFile(path));
            var auxiliaryFiles = (await Task.WhenAll(auxiliaryFilesTasks))
                .Where(fileData => fileData != null)
                .Select(fileData => fileData!)
                .ToList();

            return new FileBatch(coreFile, auxiliaryFiles);
        }

        // -------
        // Writing
        // -------

        public async Task WriteData(FileBatch newBatch, FileBatch previousBatch)
        {
            var previousManifest = await CreateManifest(previousBatch, ".manifest.old");

            var newManifest = await CreateManifest(newBatch, ".manifest.pending");
            await WriteFiles(newBatch, ".new");
            MoveManifest(newBatch, ".manifest.pending", ".manifest.new");

            RemoveManifestFiles(previousManifest, "");
            RemoveManifest(previousBatch, ".manifest.old");

            MoveManifestFiles(newManifest, ".new", "");
            RemoveManifest(newBatch, ".manifest.new");
        }

        public async Task FixWriteData(FilePath corePath)
        {
            var pendingManifest = await LoadManifest(corePath, ".manifest.pending");
            var oldManifest = await LoadManifest(corePath, ".manifest.old");
            var newManifest = await LoadManifest(corePath, ".manifest.new");

            if (oldManifest == null && pendingManifest == null && newManifest == null)
                return;

            if (pendingManifest != null)
            {
                RemoveManifestFiles(pendingManifest, ".new");
                RemoveManifest(corePath, ".manifest.pending");
                RemoveManifest(corePath, ".manifest.old");
                return;
            }

            if (newManifest == null)
            {
                RemoveManifest(corePath, ".manifest.old");
                return;
            }

            if (oldManifest != null)
            {
                RemoveManifestFiles(oldManifest, "");
                RemoveManifest(corePath, ".manifest.old");
            }

            MoveManifestFiles(newManifest, ".new", "");
            RemoveManifest(corePath, ".manifest.new");
        }

        // --------
        // Manifest
        // --------

        private async Task<FileBatchManifest> CreateManifest(FileBatch data, string suffix)
        {
            var path = data.CoreFile.Path + suffix;
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            var manifest = FileBatchManifest.FromData(data);
            var lines = manifest.Paths.Select(path => path.Value).ToArray();
            await File.WriteAllLinesAsync(path, lines);
            return manifest;
        }

        private void MoveManifest(FileBatch data, string oldSuffix, string newSuffix)
        {
            var oldPath = data.CoreFile.Path + oldSuffix;
            var newPath = data.CoreFile.Path + newSuffix;
            if (File.Exists(oldPath))
                File.Move(oldPath, newPath, overwrite: true);
        }

        private async Task<FileBatchManifest?> LoadManifest(FilePath corePath, string suffix)
        {
            var manifestPath = corePath.Value + suffix;
            if (!File.Exists(manifestPath))
                return null;

            var lines = await File.ReadAllLinesAsync(manifestPath, Encoding.UTF8);
            return FileBatchManifest.FromLines(lines);
        }

        private void RemoveManifest(FilePath corePath, string suffix)
        {
            var manifestPath = corePath.Value + suffix;
            if (File.Exists(manifestPath))
                File.Delete(manifestPath);
        }

        private void RemoveManifest(FileBatch data, string suffix)
            => RemoveManifest(data.CoreFile.Path, suffix);

        // -----
        // Files
        // -----

        private async Task WriteFiles(FileBatch data, string suffix)
        {
            foreach (var file in data.GetFiles())
            {
                var path = file.Path.Value + suffix;
                await File.WriteAllTextAsync(path, file.Content, Encoding.UTF8);
            }
        }

        private void RemoveManifestFiles(FileBatchManifest manifest, string suffix)
        {
            foreach (var path in manifest.Paths)
            {
                var fullPath = path.Value + suffix;
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
        }

        private void MoveManifestFiles(FileBatchManifest manifest, string suffixFrom, string suffixTo)
        {
            foreach (var path in manifest.Paths)
            {
                var pathFrom = path.Value + suffixFrom;
                if (!File.Exists(pathFrom))
                    continue;

                var pathTo = path.Value + suffixTo;
                File.Move(pathFrom, pathTo, overwrite: true);
            }
        }
    }
}
