using System.Linq.Expressions;
using MongoDB.Driver;

namespace TicTatToe.Data.Storage;

public abstract class MongoStorage<TEntity>
{
    protected IMongoCollection<TEntity> Collection { get; set; } = null!;

    public virtual async Task<List<TEntity>> GetAsync() =>
        await Collection.Find(_ => true).ToListAsync();

    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression) =>
        await Collection.Find(expression).FirstOrDefaultAsync();

    public virtual async Task CreateAsync(TEntity newStringData) =>
        await Collection.InsertOneAsync(newStringData);

    public virtual async Task UpdateAsync(
        Expression<Func<TEntity, bool>> findOldExpression, 
        TEntity updatedStringData) =>
        await Collection.ReplaceOneAsync(findOldExpression, updatedStringData);

    public virtual async Task RemoveAsync(Expression<Func<TEntity, bool>> expression) =>
        await Collection.DeleteOneAsync(expression);
}