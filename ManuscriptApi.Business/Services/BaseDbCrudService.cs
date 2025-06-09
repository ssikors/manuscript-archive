
using ManuscriptApi.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class BaseDbCrudService<T, TContext> : ICrudService<T>
    where T : class, IModel
    where TContext : DbContext
    {
        protected readonly TContext _context;

        public BaseDbCrudService(TContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T model, string userEmail)
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteAsync(int id, string userEmail)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T?>> GetAllAsync(string userEmail)
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, string userEmail)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T?> UpdateAsync(T model, int id, string userEmail)
        {
            var existing = await _context.Set<T>().FindAsync(id);
            if (existing == null)
                return null;

            _context.Entry(existing).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();
            return model;
        }
    }

}
