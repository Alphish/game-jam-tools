namespace Alphicsh.JamTools.Common.IO.Saving
{
    public class SaveModel<TModel, TData> : ISaveModel<TModel>
        where TModel : class
        where TData : class
    {
        private ISaveDataLoader<TModel, TData> DataLoader { get; }
        private ISaveDataExtractor<TModel, TData> DataExtractor { get; }
        private IDataSaver<TData> DataSaver { get; }

        private TData? SavedData { get; set; }
        private TData? CurrentData { get; set; }

        protected SaveModel(
            ISaveDataLoader<TModel, TData> dataLoader,
            ISaveDataExtractor<TModel, TData> dataExtractor,
            IDataSaver<TData> dataSaver
            )
        {
            DataLoader = dataLoader;
            DataExtractor = dataExtractor;
            DataSaver = dataSaver;
        }

        public bool IsModified => !object.Equals(SavedData, CurrentData);

        public void LoadSavedModel(TModel model)
        {
            SavedData = DataLoader.Load(model);
        }

        public void UpdateCurrentModel(TModel model)
        {
            CurrentData = DataExtractor.ExtractData(model);
        }

        public void DropModel()
        {
            SavedData = null;
            CurrentData = null;
        }

        public void Save(TModel model)
        {
            UpdateCurrentModel(model);
            DataSaver.Save(CurrentData!);
            SavedData = CurrentData!;
        }
    }
}
