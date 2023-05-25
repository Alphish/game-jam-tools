using Alphicsh.JamTools.Common.IO.Saving;

namespace Alphicsh.JamPackager.Model.Jam.Saving
{
    public class JamSaveModel : SaveModel<JamEditable, JamSaveData>, ISaveModel<JamEditable>
    {
        public JamSaveModel() : base(
            new JamSaveDataLoader(),
            new JamSaveDataExtractor(),
            new JamDataSaver()
            )
        {
        }
    }
}
