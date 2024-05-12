using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using System.Collections.Immutable;

namespace HardwareOnlineStore.Services.Entity.Contracts.Abstractions;

public enum TypeOfUpdateCommand
{
    Insert = 0,
    Delete = 1,
    Update = 2,
}

public interface IEntityService<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : Entities.Entity, new()
{
    Task<TEntity?> GetByAsync(TEntity condition, QueryParameters query);

    Task<IEnumerable<TEntity>?> SelectAsync(QueryParameters query);

    Task<IEnumerable<TEntity>?> SelectByAsync(TEntity conditionQueryParameters, QueryParameters query);

    Task<object?> ChangeAsync(TEntity entity, QueryParameters query);

    Task<ImmutableDictionary<string, object?>> ChangeAsync(IEnumerable<TEntity> entities, QueryParameters query);

}