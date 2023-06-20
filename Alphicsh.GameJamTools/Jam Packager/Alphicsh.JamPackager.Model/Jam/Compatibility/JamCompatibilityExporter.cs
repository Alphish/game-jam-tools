using System.Text.Json.Serialization;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamPackager.Model.Jam.Compatibility
{
    internal class JamCompatibilityExporter
    {
        private static JsonFileSaver<JamCompatibilityInfo> Saver { get; }
            = new JsonFileSaver<JamCompatibilityInfo>(JsonIgnoreCondition.Never);

        public void Export(JamEditable jam)
        {
            var path = jam.DirectoryPath.Append("jamdata.json");
            var data = JamCompatibilityInfo.FromJam(jam);
            Saver.Save(path, data);
        }
    }
}
