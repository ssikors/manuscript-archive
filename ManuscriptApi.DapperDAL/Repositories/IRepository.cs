
namespace ManuscriptApi.DapperDAL
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T model);
        Task<T?> UpdateAsync(T model, int id);
        Task<bool> DeleteAsync(int id);
    }
}
