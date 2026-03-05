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
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is missing.");

    private SqlConnection CreateConnection()
        => new(_connectionString);

    // ------------------------------
    // TRANSACTION MANAGEMENT
    // ------------------------------

    public IDbTransaction BeginTransaction()
    {
        var connection = CreateConnection();
        connection.Open();
        return connection.BeginTransaction();
    }

    public void CommitTransaction(IDbTransaction? dbTransactions = null)
    {
        if (dbTransactions == null) return;

        dbTransactions.Commit();
        dbTransactions.Connection?.Close();
        dbTransactions.Dispose();
    }

    public void RollbackTransaction(IDbTransaction? dbTransactions = null)
    {
        if (dbTransactions == null) return;

        dbTransactions.Rollback();
        dbTransactions.Connection?.Close();
        dbTransactions.Dispose();
    }

    public void Dispose(IDbTransaction? dbTransactions = null)
    {
        if (dbTransactions == null) return;

        dbTransactions.Connection?.Close();
        dbTransactions.Dispose();
    }

    // ------------------------------
    // EXECUTE (AUTO TRANSACTION)
    // ------------------------------

    public async Task<int> ExecuteAsync<P>(
        string sql,
        CommandType command,
        P parameter)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var result = await connection.ExecuteAsync(
                sql,
                parameter,
                transaction,
                commandType: command
            );

            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> ExecuteAsync<P>(
        string sql,
        CommandType command,
        P parameter,
        IDbTransaction dbTransactions)
    {
        return await dbTransactions.Connection!.ExecuteAsync(
            sql,
            parameter,
            dbTransactions,
            commandType: command
        );
    }

    // ------------------------------
    // SCALAR
    // ------------------------------

    public async Task<T> ExecuteScalarAsync<T, P>(
        string sql,
        CommandType command,
        P parameter)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var result = await connection.ExecuteScalarAsync<T>(
                sql,
                parameter,
                transaction,
                commandType: command
            );

            await transaction.CommitAsync();
            return result!;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<T> ExecuteScalarAsync<T, P>(
        string sql,
        CommandType command,
        P parameter,
        IDbTransaction dbTransactions)
    {
        var result = await dbTransactions.Connection!.ExecuteScalarAsync<T>(
            sql,
            parameter,
            dbTransactions,
            commandType: command
        );

        return result!;
    }

    // ------------------------------
    // QUERY
    // ------------------------------

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        CommandType commandType,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = CreateConnection();

        var command = new CommandDefinition(
            sql,
            parameters,
            commandType: commandType,
            cancellationToken: cancellationToken
        );

        return await connection.QueryAsync<T>(command);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        CommandType commandType,
        IDbTransaction dbTransactions,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            sql,
            parameters,
            dbTransactions,
            commandType: commandType,
            cancellationToken: cancellationToken
        );

        return await dbTransactions.Connection!.QueryAsync<T>(command);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        CommandType commandType,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = CreateConnection();

        var command = new CommandDefinition(
            sql,
            parameters,
            commandType: commandType,
            cancellationToken: cancellationToken
        );

        return await connection.QueryFirstOrDefaultAsync<T>(command);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        CommandType commandType,
        IDbTransaction dbTransactions,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            sql,
            parameters,
            dbTransactions,
            commandType: commandType,
            cancellationToken: cancellationToken
        );

        return await dbTransactions.Connection!.QueryFirstOrDefaultAsync<T>(command);
    }
}

