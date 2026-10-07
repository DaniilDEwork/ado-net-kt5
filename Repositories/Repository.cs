using System.Linq.Expressions;
using Kt5RepositoryApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Kt5RepositoryApi.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbSet<T> _entities;

    public Repository(AppDbContext context)
    {
        _entities = context.Set<T>();
    }

    public Task<List<T>> GetAllAsync()
    {
        return _entities.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return _entities.AnyAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _entities.Update(entity);
    }

    public void Delete(T entity)
    {
        _entities.Remove(entity);
    }
}
