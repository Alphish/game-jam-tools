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
    }
}
