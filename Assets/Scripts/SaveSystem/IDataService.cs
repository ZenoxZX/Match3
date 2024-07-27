namespace SaveSystem
{
    public interface IDataService
    {
        void Write();
        void Read();
        void Save(object data, IDataPersist dataPersist);
        object Load(IDataPersist dataPersist);
        bool HasData(IDataPersist dataPersist);
        void Delete(IDataPersist dataPersist);
        void DeleteAll();
    }
}