using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.Entities;

namespace HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Abstractions;

public interface IEntityWrapper<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : Entity
{
    Task<DbResponse<TEntity>> GetByIdAsync(QueryParameters query, string columnName, Guid id, CancellationToken token);

    Task<DbResponse<TEntity>> GetByAsync(QueryParameters query, TEntity entityCondition, CancellationToken token);

    Task<DbResponse<TEntity>> SelectAsync(QueryParameters query, CancellationToken token);

    Task<DbResponse<TEntity>> SelectByAsync(QueryParameters query, TEntity entityCondition, CancellationToken token);

    Task<DbResponse<TEntity>> UpdateAsync(QueryParameters query, TEntity entity, CancellationToken token);

    Task<IEnumerable<DbResponse<TEntity>>> UpdateAsync(QueryParameters query, IEnumerable<TEntity> entities, CancellationToken token);
}