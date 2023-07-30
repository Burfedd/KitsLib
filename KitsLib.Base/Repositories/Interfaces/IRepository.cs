namespace KitsLib.Base.Repositories.Interfaces
{
    public interface IRepository<T> where T:class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> InsertAsync(T entity);
        Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> entities);
    }
}
