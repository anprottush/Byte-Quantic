namespace CRUD_.Net_Core_Web_API.Interfaces
{
    public interface IRepo<TClass, TId, TResult>
    {
        ICollection<TClass> GetAll();
        TClass Get(TId id);
        TResult Create(TClass obj);
        TResult Delete(TId id);
        TResult Update(TClass obj);
    }
}
