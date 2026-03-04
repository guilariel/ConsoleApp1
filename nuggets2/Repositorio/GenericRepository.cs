using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RabbitMQAndGenericRepository.Repositorio
{
    public interface IGenericRepository<T> where T : class//buscar durable functions 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(DbContext dbContext, DbSet<T> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
             _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}


//!"#$%&/()=?¡¨*][_:;@^`~\¬/*ZZz^^`~\zx@@@
