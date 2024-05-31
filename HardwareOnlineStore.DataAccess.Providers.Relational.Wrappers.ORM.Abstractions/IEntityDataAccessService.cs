using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.Entities;

namespace HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Abstractions;

public interface IEntityWrapper<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : Entity
{
    ValueTask<DbResponse<TEntity>> GetByIdAsync(QueryParameters query, string? name, Guid? id, CancellationToken token);

    ValueTask<DbResponse<TEntity>> GetByIdsAsync(QueryParameters query, string? name, ICollection<Guid>? ids, CancellationToken token);

    ValueTask<DbResponse<TEntity>> GetByAsync(QueryParameters query, TEntity entityCondition, CancellationToken token);

    ValueTask<DbResponse<TEntity>> SelectAsync(QueryParameters query, CancellationToken token);

    ValueTask<DbResponse<TEntity>> SelectByAsync(QueryParameters query, TEntity entityCondition, CancellationToken token);

    ValueTask<DbResponse<TEntity>> UpdateAsync(QueryParameters query, TEntity entity, CancellationToken token);

    ValueTask<IEnumerable<DbResponse<TEntity>>> UpdateAsync(QueryParameters query, IEnumerable<TEntity> entities, CancellationToken token);
}