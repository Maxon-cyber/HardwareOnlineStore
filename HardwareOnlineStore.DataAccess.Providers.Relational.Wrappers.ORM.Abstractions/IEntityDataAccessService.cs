using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.Entities;

namespace HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Abstractions;

public interface IEntityWrapper<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : Entity
{
    Task<DbResponse<TEntity>> GetEntityByAsync(QueryParameters query, TEntity entityCondition, CancellationToken token);

    Task<DbResponse<TEntity>> SelectEntitiesAsync(QueryParameters query, CancellationToken token);

    Task<DbResponse<TEntity>> SelectEntitiesByAsync(QueryParameters query, TEntity entityCondition, CancellationToken token);

    Task<DbResponse<TEntity>> UpdateEntityAsync(QueryParameters query, TEntity entity, CancellationToken token);

    Task<IEnumerable<DbResponse<TEntity>>> ChangeEntityAsync(QueryParameters query, IEnumerable<TEntity> entities, CancellationToken token);
}