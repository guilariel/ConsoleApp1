using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.Repositorio
{   
    public interface IRepository<T, Type> where T : IEFEntity<Type>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Type id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
    public class EFRepository<T, Type> : IRepository<T, Type> where T : class, IEFEntity<Type>
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;
        public EFRepository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _entities.ToListAsync();
        public async Task<T?> GetByIdAsync(Type id) =>
            await _entities.FirstOrDefaultAsync(e => EqualityComparer<Type>.Default.Equals(e.key, id));
        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
