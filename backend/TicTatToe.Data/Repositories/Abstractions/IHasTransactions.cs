namespace TicTatToe.Data.Repositories.Abstractions;

public interface IHasTransactions
{
    /// <summary>
    /// Initiates a transaction scope.
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Executes the transaction.
    /// </summary>
    void CommitTransaction();

    /// <summary>
    /// Rollbacks the transaction.
    /// </summary>
    public void RollbackTransaction();
}