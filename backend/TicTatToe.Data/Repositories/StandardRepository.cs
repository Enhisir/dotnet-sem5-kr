using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicTatToe.Data.Repositories.Abstractions;
using TicTatToe.Data.Storage;

namespace TicTatToe.Data.Repositories;

public class StandardRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public StandardRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetSingleOrDefault(Expression<Func<TEntity, bool>> expression)
        => await _dbSet.AsNoTracking().SingleOrDefaultAsync(expression);

    public IQueryable<TEntity> GetRange()
    {
        return _dbSet.AsNoTracking().AsQueryable();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var newEntity = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return newEntity.Entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var updatedEntity = _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return updatedEntity.Entity;
    }

    public async Task RemoveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}