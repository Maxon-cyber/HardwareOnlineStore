﻿using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Repositories.Relational.Abstractions;
using HardwareOnlineStore.Services.Entity.Contracts.Abstractions;
using HardwareOnlineStore.Services.Utilities.Logger.Abstractions;
using System.Collections.Immutable;

namespace HardwareOnlineStore.Services.Entity.Contracts;

public abstract class EntityService<TEntity>(IRepository<TEntity> repository, ILogger logger) : IEntityService<TEntity>
    where TEntity : Entities.Entity, new()
{
    private CancellationToken _token;
    private readonly string _entityReflectionName = typeof(TEntity).Name;
    private readonly string _dbProviderReflectionName = repository.DbProviderName;
    private readonly ILogger _logger = logger;
    private readonly IRepository<TEntity> _repository = repository;

    protected virtual CancellationToken Token
    {
        get => _token == default ? default : _token;
        set
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            _token = tokenSource.Token;
        }
    }

    public async Task<TEntity?> GetByAsync(TEntity condition, QueryParameters query)
    {
        await _logger.LogInfoAsync($"Запрос на получение сущности {_entityReflectionName} из {_dbProviderReflectionName}".ToUpper());

        DbResponse<TEntity> response = await _repository.GetByAsync(query, condition, _token);

        if (response.Error != null)
        {
            await _logger.LogErrorAsync(response.Error, response.Message);
            await _logger.LogSeparatorAsync();

            return await Task.FromResult<TEntity?>(null);
        }

        await _logger.LogInfoAsync("Запрос выполнен");

        TEntity? entity = response.QueryResult.Count == 0 ? null : response.QueryResult.Peek();

        if (entity == null)
        {
            await _logger.LogInfoAsync($"Сущность {typeof(TEntity).Name} не найдена\nДоп.Данные: {response.Message}");
            return await Task.FromResult<TEntity?>(null);
        }

        await _logger.LogInfoAsync("Сущность получена:");
        await _logger.LogInfoAsync($"\tId сущности: {entity.Id}\n");

        await _logger.LogInfoAsync("Дополнительные данные:");
        if (response.AdditionalData.Count != 0)
        {
            string content = null!;
            foreach (KeyValuePair<object, object> additionalData in response.AdditionalData)
                content += $"\t{additionalData.Key}: {additionalData.Value}\n";

            await _logger.LogMessageAsync(content);
        }

        await _logger.LogSeparatorAsync();

        return entity;
    }

    public async Task<IEnumerable<TEntity>?> SelectAsync(QueryParameters query)
    {
        await _logger.LogInfoAsync($"Запрос на получение сущностей {_entityReflectionName} из {_dbProviderReflectionName}".ToUpper());

        DbResponse<TEntity> response = await _repository.SelectAsync(query, _token);

        if (response.Error != null)
        {
            await _logger.LogErrorAsync(response.Error, response.Message);
            await _logger.LogSeparatorAsync();

            return await Task.FromResult<IEnumerable<TEntity>?>(null);
        }

        await _logger.LogInfoAsync("Запрос выполнен");

        string content = null!;
        if (response.QueryResult.Count != 0)
        {
            await _logger.LogInfoAsync("Сущности получены:");

            foreach (TEntity currentEntity in response.QueryResult)
                content += $"\tId сущности: {currentEntity.Id}\n";

            await _logger.LogMessageAsync(content);
        }

        await _logger.LogInfoAsync("Дополнительные данные:");
        if (response.AdditionalData.Count != 0)
        {
            content = null!;
            foreach (KeyValuePair<object, object> additionalData in response.AdditionalData)
                content += $"\t{additionalData.Key}: {additionalData.Value}\n";

            await _logger.LogMessageAsync(content);
        }

        await _logger.LogInfoAsync(content);

        await _logger.LogSeparatorAsync();

        return response.QueryResult;
    }

    public async Task<IEnumerable<TEntity>?> SelectByAsync(TEntity condition, QueryParameters query)
    {
        await _logger.LogInfoAsync($"Запрос на получение сущностей {_entityReflectionName} из {_dbProviderReflectionName}".ToUpper());

        DbResponse<TEntity> response = await _repository.SelectByAsync(query, condition, _token);

        if (response.Error != null)
        {
            await _logger.LogErrorAsync(response.Error, response.Message);
            await _logger.LogSeparatorAsync();

            return await Task.FromResult<IEnumerable<TEntity>?>(null);
        }

        await _logger.LogInfoAsync("Запрос выполнен");
        await _logger.LogInfoAsync($"Сообщение: {response.Message}");

        string content = null!;
        if (response.QueryResult.Count != 0)
        {
            await _logger.LogInfoAsync("Сущности получены:");

            foreach (TEntity currentEntity in response.QueryResult)
                content += $"\tId сущности: {currentEntity.Id}\n";

            await _logger.LogMessageAsync(content);
        }

        await _logger.LogInfoAsync($"Сообщение: {response.Message}");

        await _logger.LogInfoAsync("Дополнительные данные:");
        if (response.AdditionalData.Count != 0)
        {
            content = null!;
            foreach (KeyValuePair<object, object> additionalData in response.AdditionalData)
                content += $"\t{additionalData.Key}: {additionalData.Value}\n";

            await _logger.LogMessageAsync(content);
        }

        await _logger.LogInfoAsync(content);

        await _logger.LogSeparatorAsync();

        return response.QueryResult;
    }

    public async Task<object?> ChangeAsync(TEntity entity, QueryParameters query)
    {
        await _logger.LogInfoAsync($"Запрос на изменение cущности {_entityReflectionName} из {_dbProviderReflectionName}".ToUpper());
        await _logger.LogInfoAsync("Количество сущностей на изменение: 1");

        DbResponse<TEntity> response = await _repository.ChangeAsync(query, entity, _token);

        if (response.Error != null)
        {
            await _logger.LogErrorAsync(response.Error, response.Message);
            await _logger.LogSeparatorAsync();

            return await Task.FromResult<object?>(null);
        }

        await _logger.LogInfoAsync("Запрос выполнен");
        await _logger.LogInfoAsync($"Сообщение: {response.Message}");

        await _logger.LogInfoAsync("Дополнительные данные:");
        if (response.AdditionalData.Count != 0)
        {
            string content = null!;
            foreach (KeyValuePair<object, object> additionalData in response.AdditionalData)
                content += $"\t{additionalData.Key}: {additionalData.Value}\n";

            await _logger.LogMessageAsync(content);
        }

        await _logger.LogInfoAsync($"Выходное значение: {response.OutputValue}");
        await _logger.LogInfoAsync($"Возвращаемое значение: {response.ReturnedValue}");

        await _logger.LogSeparatorAsync();

        return response.ReturnedValue;
    }

    public async Task<ImmutableDictionary<string, object?>> ChangeAsync(IEnumerable<TEntity> entities, QueryParameters query)
    {
        await _logger.LogInfoAsync($"Запрос на изменение cущности {_entityReflectionName} из {_dbProviderReflectionName}".ToUpper());
        await _logger.LogInfoAsync($"Количество сущностей на изменение: {entities.Count()}");

        IEnumerable<DbResponse<TEntity>> responses = await _repository.ChangeAsync(query, entities, _token);

        ImmutableDictionary<string, object?> result = ImmutableDictionary.Create<string, object?>();

        DbResponse<TEntity>[] responsesArray = responses.ToArray();

        for (int index = 0; index < responsesArray.Length; index++)
        {
            result.Add($"Сущность: {index}", null);

            DbResponse<TEntity> response = responsesArray[index];

            await _logger.LogInfoAsync($"Сообщение: {response.Message}");
            if (response.Error != null)
            {
                await _logger.LogErrorAsync(response.Error, response.Message);
                await _logger.LogSeparatorAsync();
                result.Add($"Ошибка {index}", response.OutputValue);
                continue;
            }

            await _logger.LogInfoAsync("Запрос выполнен");

            await _logger.LogInfoAsync("Дополнительные данные:");
            if (response.AdditionalData.Count != 0)
            {
                string content = null!;
                foreach (KeyValuePair<object, object> additionalData in response.AdditionalData)
                    content += $"\t{additionalData.Key}: {additionalData.Value}\n";

                await _logger.LogMessageAsync(content);
            }

            await _logger.LogInfoAsync($"Выходное значение: {response.OutputValue}");
            await _logger.LogInfoAsync($"Возвращаемое значениеЖ {response.ReturnedValue}");

            result.Add("Выходное значение", response.OutputValue);
            result.Add("Возвращаемое значение", response.ReturnedValue);
        }

        await _logger.LogSeparatorAsync();

        return result;
    }

    public void Dispose()
    {
        GC.Collect();
        GC.SuppressFinalize(this);

        _repository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        GC.Collect();
        GC.SuppressFinalize(this);

        await _repository.DisposeAsync();
    }
}