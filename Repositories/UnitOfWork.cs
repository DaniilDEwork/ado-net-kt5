using Kt5RepositoryApi.Data;
using Kt5RepositoryApi.Models;

namespace Kt5RepositoryApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRepository<Category> Categories { get; }
    public IRepository<Product> Products { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Categories = new Repository<Category>(context);
        Products = new Repository<Product>(context);
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
