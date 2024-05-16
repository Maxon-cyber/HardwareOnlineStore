using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Abstractions;
using HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Implementations.ADO.Exceptions;
using HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Implementations.ADO.Extensions;
using HardwareOnlineStore.Entities;
using System.Data.Common;

namespace HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Implementations.ADO;

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

    public async Task<DbResponse<TEntity>> GetByIdAsync(QueryParameters query, string columnName, Guid id, CancellationToken token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(columnName);

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
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, _prefix, query.OutputParameter.DbType, parameterDirection: query.OutputParameter.ParameterDirection, size: query.OutputParameter.Size);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, _prefix, query.ReturnedValue.DbType, parameterDirection: query.ReturnedValue.ParameterDirection, size: query.ReturnedValue.Size);

            _dbCommand.AddWithValue(columnName, _prefix, System.Data.DbType.Guid, id);
            
            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
                while (await reader.ReadAsync(token))
                    response.QueryResult.Enqueue(await reader.MappingAsync<TEntity>());

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async Task<DbResponse<TEntity>> GetByAsync(QueryParameters query, TEntity condition, CancellationToken token)
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
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, _prefix, query.OutputParameter.DbType, parameterDirection: query.OutputParameter.ParameterDirection, size: query.OutputParameter.Size);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, _prefix, query.ReturnedValue.DbType, parameterDirection: query.ReturnedValue.ParameterDirection, size: query.ReturnedValue.Size);

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

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async Task<DbResponse<TEntity>> SelectAsync(QueryParameters query, CancellationToken token)
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
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, _prefix, query.OutputParameter.DbType, parameterDirection: query.OutputParameter.ParameterDirection, size: query.OutputParameter.Size);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, _prefix, query.ReturnedValue.DbType, parameterDirection: query.ReturnedValue.ParameterDirection, size: query.ReturnedValue.Size);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
                while (await reader.ReadAsync(token))
                    response.QueryResult.Enqueue(await reader.MappingAsync<TEntity>());

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async Task<DbResponse<TEntity>> SelectByAsync(QueryParameters query, TEntity condition, CancellationToken token)
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
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, _prefix, query.OutputParameter.DbType, parameterDirection: query.OutputParameter.ParameterDirection, size: query.OutputParameter.Size);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, _prefix, query.ReturnedValue.DbType, parameterDirection: query.ReturnedValue.ParameterDirection, size: query.ReturnedValue.Size);

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

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async Task<DbResponse<TEntity>> UpdateAsync(QueryParameters query, TEntity entity, CancellationToken token)
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
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, _prefix, query.OutputParameter.DbType, parameterDirection: query.OutputParameter.ParameterDirection, size: query.OutputParameter.Size);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, _prefix, query.ReturnedValue.DbType, parameterDirection: query.ReturnedValue.ParameterDirection, size: query.ReturnedValue.Size);

            int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(entity, _prefix);

            int countOfAddedRows = await _dbCommand.ExecuteNonQueryAsync(token);

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество переданных параметров для изменения сущности", countOfAddedValues);
            response.AdditionalData.TryAdd("Количество обновленных строк", countOfAddedRows);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.AdditionalData.TryAdd("Выходной параметр", response?.OutputValue);
            response.AdditionalData.TryAdd("Возвращаемое значение", response?.ReturnedValue);

            response.Message = response.OutputValue.ToString();

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async Task<IEnumerable<DbResponse<TEntity>>> UpdateAsync(QueryParameters query, IEnumerable<TEntity> entities, CancellationToken token)
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
                outputDbParameter = _dbCommand.AddWithValue(query.OutputParameter.Name, _prefix, query.OutputParameter.DbType, parameterDirection: query.OutputParameter.ParameterDirection, size: query.OutputParameter.Size);

            if (query.ReturnedValue != null)
                returnedDbParameter = _dbCommand.AddWithValue(query.ReturnedValue.Name, _prefix, query.ReturnedValue.DbType, parameterDirection: query.ReturnedValue.ParameterDirection, size: query.ReturnedValue.Size);

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

                await dbTransaction.CommitTransactionAsync(token);
            }
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
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