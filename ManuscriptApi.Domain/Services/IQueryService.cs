using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuscriptApi.Domain.Services
{
    public interface IQueryService<T> : IService<T> where T : IModel
    {
        Task<T?> GetByIdAsync(int id, string userEmail);
        Task<IEnumerable<T?>> GetAllAsync(string userEmail);
    }
}
