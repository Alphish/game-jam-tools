namespace Alphicsh.JamTools.Common.IO.Saving
{
    public class SaveModel<TModel, TData> : ISaveModel<TModel>
        where TModel : class
        where TData : class
    {
        private ISaveDataExtractor<TModel, TData> DataExtractor { get; } = default!;
        private IDataSaver<TData> DataSaver { get; } = default!;

        private TData? LastSavedData { get; set; }
        private TData? CurrentData { get; set; }

        protected SaveModel(ISaveDataExtractor<TModel, TData> dataExtractor, IDataSaver<TData> dataSaver)
        {
            DataExtractor = dataExtractor;
            DataSaver = dataSaver;
        }

        public bool IsModified => !object.Equals(LastSavedData, CurrentData);

        public void AcceptModel(TModel model)
        {
            LastSavedData = DataExtractor.ExtractData(model);
            CurrentData = LastSavedData;
        }

        public void ChangeModel(TModel model)
        {
            CurrentData = DataExtractor.ExtractData(model);
        }

        public void DropModel()
        {
            LastSavedData = null;
            CurrentData = null;
        }

        public void Save(TModel model)
        {
            AcceptModel(model);
            DataSaver.Save(CurrentData!);
        }
    }
}
