using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.Entities;

namespace HardwareOnlineStore.DataAccess.Repositories.Relational.Abstractions;

public interface IRepository<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : Entity
{
    string DbProviderName { get; }

    Task<DbResponse<TEntity>> GetByAsync(QueryParameters queryParameters, TEntity entityCondition, CancellationToken token);

    Task<DbResponse<TEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token);

    Task<DbResponse<TEntity>> SelectByAsync(QueryParameters queryParameters, TEntity userCondition, CancellationToken token);

    Task<DbResponse<TEntity>> ChangeAsync(QueryParameters queryParameters, TEntity user, CancellationToken token);

    Task<IEnumerable<DbResponse<TEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<TEntity> users, CancellationToken token);
}