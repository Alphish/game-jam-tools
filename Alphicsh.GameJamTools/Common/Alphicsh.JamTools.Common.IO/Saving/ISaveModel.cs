namespace Alphicsh.JamTools.Common.IO.Saving
{
    public interface ISaveModel<TModel>
    {
        bool HasChanges { get; }
        void DropModel();
        void AcceptModel(TModel model);
        void ChangeModel(TModel model);
        void Save(TModel model);
    }
}
