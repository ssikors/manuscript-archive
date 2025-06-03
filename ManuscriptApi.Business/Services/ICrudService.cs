using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.DataAccess.Models;

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
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T?>> GetAllAsync();
    }

    public interface ICrudService<T> : IQueryService<T> where T : IModel
    {
        Task<T> CreateAsync(T model);
        Task<T?> UpdateAsync(T model, int id);
        Task<bool> DeleteAsync(int id);
    }
}
