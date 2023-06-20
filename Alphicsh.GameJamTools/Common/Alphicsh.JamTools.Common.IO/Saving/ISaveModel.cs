namespace Alphicsh.JamTools.Common.IO.Saving
{
    public interface ISaveModel<TModel>
    {
        bool IsModified { get; }
        void DropModel();
        void LoadSavedModel(TModel model);
        void UpdateCurrentModel(TModel model);
        void Save(TModel model);
        void MarkUnmodified();
    }
}
