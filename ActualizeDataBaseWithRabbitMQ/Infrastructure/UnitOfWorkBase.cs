using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;

public abstract class UnitOfWorkBase : IDisposable
{
    protected readonly GenericDbContext _context;

    protected UnitOfWorkBase(string connectionString)
    {
       var options = new DbContextOptionsBuilder<GenericDbContext>()
            .UseNpgsql(connectionString)
            .Options;

       _context = new GenericDbContext(options);
    }

    public int SaveChanges() => _context.SaveChanges();
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
