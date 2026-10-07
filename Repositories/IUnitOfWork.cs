using Kt5RepositoryApi.Models;

namespace Kt5RepositoryApi.Repositories;

public interface IUnitOfWork
{
    IRepository<Category> Categories { get; }
    IRepository<Product> Products { get; }
    Task<int> SaveChangesAsync();
}
