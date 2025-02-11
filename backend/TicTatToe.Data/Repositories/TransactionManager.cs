using Microsoft.EntityFrameworkCore.Storage;
using TicTatToe.Data.Repositories.Abstractions;
using TicTatToe.Data.Storage;

namespace TicTatToe.Data.Repositories;

public class TransactionManager(AppDbContext context) : IHasTransactions
{
    private IDbContextTransaction? _transaction;

    public void BeginTransaction()
    {
        _transaction = context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        if (_transaction is null) throw new InvalidOperationException();
        _transaction.Commit();
    }

    public void RollbackTransaction()
    {
        if (_transaction is null) throw new InvalidOperationException();
        _transaction.Rollback();
    }
}