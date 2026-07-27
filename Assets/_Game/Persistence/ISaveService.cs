namespace Asteria.Persistence
{
    public interface ISaveService
    {
        SaveRoot Current { get; }
        void LoadOrCreate();
        void Save();
    }
}
