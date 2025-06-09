
using ManuscriptApi.DapperDAL;
using ManuscriptApi.Domain.Services;
using Microsoft.Extensions.Logging;

namespace ManuscriptApi.Business.Services
{
    public class DapperCrudService<T> : ICrudService<T> where T : class, IModel
    {
        protected readonly IRepository<T> _repository;
        private readonly ILogger<DapperCrudService<T>> _logger;

        public DapperCrudService(IRepository<T> repository, ILogger<DapperCrudService<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<T> CreateAsync(T model, string userEmail)
        {
            _logger.LogInformation("User {UserEmail} is creating a new {EntityType}", userEmail, typeof(T).Name);

            var entity = await _repository.CreateAsync(model);

            _logger.LogInformation("User {UserEmail} created {EntityType} with ID {EntityId}", userEmail, typeof(T).Name, entity?.Id);

            return entity;
        }

        public async Task<bool> DeleteAsync(int id, string userEmail)
        {
            _logger.LogInformation("User {UserEmail} is deleting {EntityType} with ID {EntityId}", userEmail, typeof(T).Name, id);

            var deleted = await _repository.DeleteAsync(id);

            _logger.LogInformation("User {UserEmail} deletion of {EntityType} ID {EntityId} result: {Deleted}", userEmail, typeof(T).Name, id, deleted);

            return deleted;
        }

        public async Task<IEnumerable<T?>> GetAllAsync(string userEmail)
        {
            _logger.LogInformation("User {UserEmail} is retrieving all {EntityType} records", userEmail, typeof(T).Name);

            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(int id, string userEmail)
        {
            _logger.LogInformation("User {UserEmail} is retrieving {EntityType} with ID {EntityId}", userEmail, typeof(T).Name, id);

            return await _repository.GetByIdAsync(id);
        }

        public async Task<T?> UpdateAsync(T model, int id, string userEmail)
        {
            _logger.LogInformation("User {UserEmail} is updating {EntityType} with ID {EntityId}", userEmail, typeof(T).Name, id);

            var updated = await _repository.UpdateAsync(model, id);

            _logger.LogInformation("User {UserEmail} updated {EntityType} with ID {EntityId}", userEmail, typeof(T).Name, updated?.Id);

            return updated;
        }
    }

}
