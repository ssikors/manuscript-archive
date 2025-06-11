using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuscriptApi.Domain.Services
{
    public interface ICrudService<T> : IQueryService<T> where T : IModel
    {
        Task<T> CreateAsync(T model, string userEmail);
        Task<T?> UpdateAsync(T model, int id, string userEmail);
        Task<bool> DeleteAsync(int id, string userEmail);
    }
}
