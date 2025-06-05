
using ManuscriptApi.DataAccess.Data.Repositories;
using ManuscriptApi.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class DapperCrudService<T> : ICrudService<T>
    where T : class, IModel
    {
        protected readonly IRepository<T> _repository;

        public DapperCrudService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> CreateAsync(T model)
        {
            var entity = await _repository.CreateAsync(model);

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool deleted = await _repository.DeleteAsync(id);

            return deleted;
        }

        public async Task<IEnumerable<T?>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T?> UpdateAsync(T model, int id)
        {
            T? updated = await _repository.UpdateAsync(model, id);
            
            return updated;
        }
    }
}
