using System.Data;
using Dapper;

namespace SqlDataAccess;

internal enum IDbConnectionState { Open, Closed }

public interface ISqlDataAccess
{
    IDbTransaction BeginTransaction();
    void CommitTransaction(IDbTransaction? dbTransactions = null);
    void Dispose(IDbTransaction? dbTransactions = null);
    Task<int> ExecuteAsync<P>(string sql, CommandType command, P parameter);
    Task<int> ExecuteAsync<P>(string sql, CommandType command, P parameter, IDbTransaction dbTransactions);
    Task<T> ExecuteScalarAsync<T, P>(string sql, CommandType command, P parameter);
    Task<T> ExecuteScalarAsync<T, P>(string sql, CommandType command, P parameter, IDbTransaction dbTransactions);
    Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, IDbTransaction dbTransactions, object? parameters = null, CancellationToken cancellationToken = default);
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default);
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, CommandType commandType, IDbTransaction dbTransactions, object? parameters = null, CancellationToken cancellationToken = default);
    void RollbackTransaction(IDbTransaction? dbTransactions = null);
}

public class SqlDataAccess(IDbConnection dbConnection) : ISqlDataAccess
{
    private readonly IDbConnection dbConnection = dbConnection;
    private IDbTransaction? dbTransaction = null;
    private IDbConnectionState? dbConnectionState = null;

    public IDbTransaction BeginTransaction()
    {
        dbConnection.Open();
        dbConnectionState = IDbConnectionState.Open;

        dbTransaction = dbConnection.BeginTransaction();
        return dbTransaction;
    }

    public void CommitTransaction(IDbTransaction? dbTransactions = null)
    {
        var transaction = dbTransactions ?? dbTransaction;
        transaction?.Commit();

        dbConnection.Close();
        dbConnectionState = IDbConnectionState.Closed;
    }

    public void RollbackTransaction(IDbTransaction? dbTransactions = null)
    {
        var transaction = dbTransactions ?? dbTransaction;
        transaction?.Rollback();

        dbConnection.Close();
        dbConnectionState = IDbConnectionState.Closed;
    }

    public void Dispose(IDbTransaction? dbTransactions = null)
    {
        var transaction = dbTransactions ?? dbTransaction;

        if (dbConnectionState == IDbConnectionState.Open)
        {
            CommitTransaction();

        }
        transaction?.Dispose();
        dbConnection.Dispose();
        dbConnectionState = IDbConnectionState.Closed;

    }

    public async Task<int> ExecuteAsync<P>(string sql, CommandType command, P parameter)
    {
        BeginTransaction();
        try
        {
            var response = await dbConnection.ExecuteAsync(
                sql,
                parameter,
                commandType: command,
                transaction: dbTransaction
            )!;

            if (response > 0)
            {
                CommitTransaction();
                return 1;
            }

            return 0;
        }
        catch (Exception)
        {
            RollbackTransaction();
            return 0;
        }
        finally
        {
            Dispose();
        }
    }

    public async Task<int> ExecuteAsync<P>(string sql, CommandType command, P parameter, IDbTransaction dbTransactions)
    {
        try
        {
            var response = await dbConnection.ExecuteAsync(
                sql,
                parameter,
                commandType: command,
                transaction: dbTransactions
            )!;

            if (response > 0)
            {
                return 1;
            }

            return 0;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task<T> ExecuteScalarAsync<T, P>(string sql, CommandType command, P parameter)
    {
        BeginTransaction();
        try
        {
            var response = await dbConnection.ExecuteScalarAsync<T>(
                sql,
                parameter,
                commandType: command,
                transaction: dbTransaction
            )!;

            if (response != null)
            {
                CommitTransaction();
                return response ?? default!;
            }

            return default!;
        }
        catch (Exception)
        {
            RollbackTransaction();
            return default!;
        }
        finally
        {
            Dispose();
        }
    }

    public async Task<T> ExecuteScalarAsync<T, P>(string sql, CommandType command, P parameter, IDbTransaction dbTransactions)
    {
        try
        {
            var response = await dbConnection.ExecuteScalarAsync<T>(
                sql,
                parameter,
                commandType: command,
                transaction: dbTransactions
            )!;

            if (response != null)
            {
                return response ?? default!;
            }

            return default!;
        }
        catch (Exception)
        {
            return default!;
        }
    }

    public Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(sql,
                                            parameters,
                                            dbTransaction,
                                            commandType: commandType,
                                            cancellationToken: cancellationToken);

        return dbConnection.QueryAsync<T>(command);
    }

    public Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, IDbTransaction dbTransactions, object? parameters = null, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(sql,
                                             parameters,
                                             dbTransactions,
                                             commandType: commandType,
                                             cancellationToken: cancellationToken);

        return dbConnection.QueryAsync<T>(command);
    }

    public Task<T?> QueryFirstOrDefaultAsync<T>(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(sql,
                                             parameters,
                                             dbTransaction,
                                             commandType: commandType,
                                             cancellationToken: cancellationToken);

        return dbConnection.QueryFirstOrDefaultAsync<T>(command);
    }

    public Task<T?> QueryFirstOrDefaultAsync<T>(string sql, CommandType commandType, IDbTransaction dbTransactions, object? parameters = null, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(sql,
                                             parameters,
                                             dbTransactions,
                                             commandType: commandType,
                                             cancellationToken: cancellationToken);

        return dbConnection.QueryFirstOrDefaultAsync<T>(command);
    }

}
