using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;

public abstract class UnitOfWorkBase : IDisposable
{
    protected readonly DbContext _context;

    protected UnitOfWorkBase(DbContext dbContext)
    {
        _context = dbContext;
    }

    public int SaveChanges() => _context.SaveChanges();
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
