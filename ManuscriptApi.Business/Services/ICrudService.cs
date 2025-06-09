
namespace ManuscriptApi.Business.Services
{
    public interface IService<T> : IService where T : IModel
    {

    }

    public interface IService
    {

    }

    public interface IQueryService<T> : IService<T> where T : IModel
    {
        Task<T?> GetByIdAsync(int id, string userEmail);
        Task<IEnumerable<T?>> GetAllAsync(string userEmail);
    }

    public interface ICrudService<T> : IQueryService<T> where T : IModel
    {
        Task<T> CreateAsync(T model, string userEmail);
        Task<T?> UpdateAsync(T model, int id, string userEmail);
        Task<bool> DeleteAsync(int id, string userEmail);
    }
}
