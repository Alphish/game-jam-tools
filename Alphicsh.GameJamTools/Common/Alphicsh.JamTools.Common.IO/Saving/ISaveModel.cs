namespace Alphicsh.JamTools.Common.IO.Saving
{
    public interface ISaveModel<TModel>
    {
        bool IsModified { get; }
        void DropModel();
        void AcceptModel(TModel model);
        void ChangeModel(TModel model);
        void Save(TModel model);
    }
}
