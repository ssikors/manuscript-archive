using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuscriptApi.DataAccess.Data.Repositories
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
