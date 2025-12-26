namespace RabbitMQAndGenericRepository.Repositorio
{
    public interface IGenericRepository<T, IKey> where T : IEntitys<IKey>//buscar durable functions 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(IKey key);
        Task<T> GetByKeyAsync(IKey key);
        Task<bool> ContainsEntityAsync(IKey key);
    }
    public class GenericRepository<T, IKey> : IGenericRepository<T, IKey> where T : IEntitys<IKey>
    {
        protected List<T> _entities { get; set; } = new List<T>();
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_entities.AsEnumerable());
        }
        public Task AddAsync(T entity)
        {
            if (_entities.Contains(entity))
                throw new ArgumentException("The entity already exists in the repository.");
            _entities.Add(entity);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(T entity)
        {
            var index = _entities.FindIndex(e => e.Equals(entity));
            if (index == -1)
                throw new ArgumentException("The entity does not exist in the repository.");
            _entities[index] = entity;
            return Task.CompletedTask;
        }
        public Task DeleteAsync(IKey key)
        {
            var entity = _entities.FirstOrDefault(e => EqualityComparer<IKey>.Default.Equals(e.Key, key));
            if (entity == null)
                throw new ArgumentException("The entity does not exist in the repository.");
            _entities.Remove(entity);
            return Task.CompletedTask;
        }
        public Task<T> GetByKeyAsync(IKey key)
        {
            var entity = _entities.FirstOrDefault(e => EqualityComparer<IKey>.Default.Equals(e.Key, key));
            if (entity == null)
                throw new ArgumentException("The entity does not exist in the repository.");
            return Task.FromResult(entity);
        }
        public Task<bool> ContainsEntityAsync(IKey key)
        {
            bool exists = _entities.Any(e => EqualityComparer<IKey>.Default.Equals(e.Key, key));
            return Task.FromResult(exists);
        }
    }
}