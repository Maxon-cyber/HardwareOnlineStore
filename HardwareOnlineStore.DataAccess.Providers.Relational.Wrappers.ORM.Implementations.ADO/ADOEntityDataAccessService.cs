using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Abstractions;
using HardwareOnlineStore.Entities;
using HardwareStore.Providers.Relational.DataTools.ADO.Exceptions;
using HardwareStore.Providers.Relational.DataTools.ADO.Extensions;
using System.Data.Common;

namespace HardwareStore.Providers.Relational.DataTools.ADO;

file enum ListOfSupportedDbProviders
{
    SqlServer = 0,
    Oracle = 1,
    MySQL = 2,
    PostgreSQL = 3,
    SQLite = 4,
    Firebird = 5,
    IBMDB2 = 6,
    Informix = 7,
    SQLServerCompactEdition = 8,
    MicrosoftAccess = 9,
    ODBC = 10,
    OLEDB = 11,
    DevartOracle = 12
}

public sealed class ADOEntityDataAccessService<TEntity> : IEntityWrapper<TEntity>
    where TEntity : Entity
{
    private readonly string _prefix;
    private readonly DbConnection _dbConnection;
    private readonly DbCommand _dbCommand;

    public ADOEntityDataAccessService(string provider, string prefix, DbConnection dbConnection, DbCommand dbCommand)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(provider));

        if (provider.Contains("Provider"))
            provider = provider.Replace("Provider", "");

        if (!Enum.TryParse(provider, true, out ListOfSupportedDbProviders _))
            throw new UnsupportedDbProviderException($"Провайдер {provider} не поддерживается технологией ADO.Net");

        _prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _dbCommand = dbCommand ?? throw new ArgumentNullException(nameof(dbCommand));
    }

    public async Task<DbResponse<TEntity>> GetEntityByAsync(QueryParameters query, TEntity condition, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            DbParameter? outputDbParameter = null;
            DbParameter? returnedDbParameter = null;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            if (query.OutputParameter != null)
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, query.OutputParameter.Size, query.OutputParameter.DbType, query.OutputParameter.ParameterDirection, _prefix);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, query.ReturnedValue.Size, query.ReturnedValue.DbType, query.ReturnedValue.ParameterDirection, _prefix);

            int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(condition, _prefix);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
                while (await reader.ReadAsync(token))
                    response.QueryResult.Enqueue(await reader.MappingAsync<TEntity>());

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            if (dbTransaction != null)
                await dbTransaction.CommitAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            if (dbTransaction != null)
                await dbTransaction.DisposeAsync();
        }

        return response;
    }

    public async Task<DbResponse<TEntity>> SelectEntitiesAsync(QueryParameters query, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            DbParameter? outputDbParameter = null;
            DbParameter? returnedDbParameter = null;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            if (query.OutputParameter != null)
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, query.OutputParameter.Size, query.OutputParameter.DbType, query.OutputParameter.ParameterDirection, _prefix);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, query.ReturnedValue.Size, query.ReturnedValue.DbType, query.ReturnedValue.ParameterDirection, _prefix);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
                while (await reader.ReadAsync(token))
                    response.QueryResult.Enqueue(await reader.MappingAsync<TEntity>());

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            if (dbTransaction != null)
                await dbTransaction.CommitAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);

        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);

        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            if (dbTransaction != null)
                await dbTransaction.DisposeAsync();
        }

        return response;
    }

    public async Task<DbResponse<TEntity>> SelectEntitiesByAsync(QueryParameters query, TEntity condition, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            DbParameter? outputDbParameter = null;
            DbParameter? returnedDbParameter = null;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            if (query.OutputParameter != null)
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, query.OutputParameter.Size, query.OutputParameter.DbType, query.OutputParameter.ParameterDirection, _prefix);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, query.ReturnedValue.Size, query.ReturnedValue.DbType, query.ReturnedValue.ParameterDirection, _prefix);

            int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(condition, _prefix);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
                while (await reader.ReadAsync(token))
                    response.QueryResult.Enqueue(await reader.MappingAsync<TEntity>());

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            if (dbTransaction != null)
                await dbTransaction.CommitAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);

        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            if (dbTransaction != null)
                await dbTransaction.DisposeAsync();
        }

        return response;
    }

    public async Task<DbResponse<TEntity>> UpdateEntityAsync(QueryParameters query, TEntity entity, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            DbParameter? outputDbParameter = null;
            DbParameter? returnedDbParameter = null;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            if (query.OutputParameter != null)
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, query.OutputParameter.Size, query.OutputParameter.DbType, query.OutputParameter.ParameterDirection, _prefix);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, query.ReturnedValue.Size, query.ReturnedValue.DbType, query.ReturnedValue.ParameterDirection, _prefix);

            int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(entity, _prefix);

            int countOfAddedRows = await _dbCommand.ExecuteNonQueryAsync(token);

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество переданных параметров для изменения сущности", countOfAddedValues);
            response.AdditionalData.TryAdd("Количество обновленных строк", countOfAddedRows);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            if (dbTransaction != null)
                await dbTransaction.CommitAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            if (dbTransaction != null)
                await dbTransaction.DisposeAsync();
        }

        return response;
    }

    public async Task<IEnumerable<DbResponse<TEntity>>> ChangeEntityAsync(QueryParameters query, IEnumerable<TEntity> entities, CancellationToken token)
    {
        List<DbResponse<TEntity>> responses = [];

        DbTransaction? dbTransaction = null;
        DbResponse<TEntity>? response = new DbResponse<TEntity>();

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            DbParameter? outputDbParameter = null;
            DbParameter? returnedDbParameter = null;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            if (query.OutputParameter != null)
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, query.OutputParameter.Size, query.OutputParameter.DbType, query.OutputParameter.ParameterDirection, _prefix);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, query.ReturnedValue.Size, query.ReturnedValue.DbType, query.ReturnedValue.ParameterDirection, _prefix);

            response.AdditionalData.TryAdd("Тип сущнотсти", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество переданных сущностей для изменения", entities.Count());

            foreach (TEntity entity in entities)
            {
                response = new DbResponse<TEntity>();

                int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(entity, _prefix);

                int countOfAddedRows = await _dbCommand.ExecuteNonQueryAsync(token);

                response.AdditionalData.TryAdd("Количество переданных параметров для изменения сущности", countOfAddedValues);
                response.AdditionalData.TryAdd("Количество обновленных строк", countOfAddedRows);

                response.OutputValue = outputDbParameter?.Value;
                response.ReturnedValue = returnedDbParameter?.Value;

                response.Message = "Запрос выполнен";

                responses.Add(response);
            }

            if (dbTransaction != null)
                await dbTransaction.CommitAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;

            if (dbTransaction != null)
                await dbTransaction.RollbackAsync(token);
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            if (dbTransaction != null)
                await dbTransaction.DisposeAsync();
        }

        return responses;
    }

    public void Dispose()
    {
        GC.Collect();
        GC.SuppressFinalize(this);

        _dbConnection.Dispose();
        _dbCommand.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        GC.Collect();
        GC.SuppressFinalize(this);

        await _dbConnection.DisposeAsync();
        await _dbCommand.DisposeAsync();
    }
}