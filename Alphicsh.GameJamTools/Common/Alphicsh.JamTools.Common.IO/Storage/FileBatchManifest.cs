using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTools.Common.IO.Storage
{
    internal class FileBatchManifest
    {
        public IReadOnlyCollection<FilePath> Paths { get; }

        private FileBatchManifest(IEnumerable<FilePath> paths)
        {
            Paths = paths.ToList();
        }

        public static FileBatchManifest FromData(FileBatch data)
        {
            var entries = data.GetFiles().Select(file => file.Path);
            return new FileBatchManifest(entries);
        }

        public static FileBatchManifest? FromLines(IEnumerable<string> lines)
        {
            var relevantLines = lines.Where(line => line.Trim() != "").ToList();
            var paths = relevantLines.Select(line => FilePath.From(line.Trim())).ToList();
            return new FileBatchManifest(paths);
        }
    }
}
