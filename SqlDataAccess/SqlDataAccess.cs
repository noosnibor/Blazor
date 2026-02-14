using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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

public class SqlDataAccess(IConfiguration configuration) : ISqlDataAccess
{
    private IDbConnection? dbConnection;
    private IDbTransaction? dbTransaction = null;
    private IDbConnectionState? dbConnectionState = null;
    private readonly string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string missing.");

    public IDbTransaction BeginTransaction()
    {
        dbConnection = new SqlConnection(connectionString);
        dbConnection.Open();
        dbConnectionState = IDbConnectionState.Open;

        dbTransaction = dbConnection.BeginTransaction();
        return dbTransaction;
    }

    public void CommitTransaction(IDbTransaction? dbTransactions = null)
    {
        var transaction = dbTransactions ?? dbTransaction;
        transaction?.Commit();

        dbConnection?.Close();
        dbConnectionState = IDbConnectionState.Closed;
    }

    public void RollbackTransaction(IDbTransaction? dbTransactions = null)
    {
        var transaction = dbTransactions ?? dbTransaction;
        transaction?.Rollback();

        dbConnection?.Close();
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
        dbConnection?.Dispose();
        dbConnectionState = IDbConnectionState.Closed;

    }

    public async Task<int> ExecuteAsync<P>(string sql, CommandType command, P parameter)
    {
        BeginTransaction();
        try
        {
            var response = await dbConnection?.ExecuteAsync(
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
            var response = await dbConnection?.ExecuteAsync(
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
            var response = await dbConnection?.ExecuteScalarAsync<T>(
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
            var response = await dbConnection?.ExecuteScalarAsync<T>(
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

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        var command = new CommandDefinition(sql,
                                            parameters,
                                            commandType: commandType,
                                            cancellationToken: cancellationToken);

        return await connection!.QueryAsync<T>(command);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, IDbTransaction dbTransactions, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        var command = new CommandDefinition(sql,
                                             parameters,
                                             dbTransactions,
                                             commandType: commandType,
                                             cancellationToken: cancellationToken);

        return await connection!.QueryAsync<T>(command);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        var command = new CommandDefinition(sql,
                                             parameters,
                                             commandType: commandType,
                                             cancellationToken: cancellationToken);

        return await connection!.QueryFirstOrDefaultAsync<T>(command);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, CommandType commandType, IDbTransaction dbTransactions, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = new SqlConnection(connectionString);
        var command = new CommandDefinition(sql,
                                             parameters,
                                             dbTransactions,
                                             commandType: commandType,
                                             cancellationToken: cancellationToken);

        return await connection!.QueryFirstOrDefaultAsync<T>(command);
    }

}
