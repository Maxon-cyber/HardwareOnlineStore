using HardwareOnlineStore.DataAccess.Providers.Relational.Implementations.SqlServer;
using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Repositories.Relational.Abstractions;
using HardwareOnlineStore.Entities.Order;

namespace HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Order;

public sealed class OrderRepository(SqlServerProvider<OrderEntity> sqlServer) : IRepository<OrderEntity>
{
    public string DbProviderName => "SqlServer";

    public Task<DbResponse<OrderEntity>> GetByAsync(QueryParameters queryParameters, OrderEntity orderCondition, CancellationToken token)
        => sqlServer.GetValueByAsync(queryParameters, orderCondition, token);

    public Task<DbResponse<OrderEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => sqlServer.SelectValuesAsync(queryParameters, token);

    public Task<DbResponse<OrderEntity>> SelectByAsync(QueryParameters queryParameters, OrderEntity orderCondition, CancellationToken token)
        => sqlServer.SelectValuesByAsync(queryParameters, orderCondition, token);

    public Task<DbResponse<OrderEntity>> ChangeAsync(QueryParameters queryParameters, OrderEntity order, CancellationToken token)
        => sqlServer.ChangeValueAsync(queryParameters, order, token);

    public Task<IEnumerable<DbResponse<OrderEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<OrderEntity> orders, CancellationToken token)
        => sqlServer.ChangeValuesAsync(queryParameters, orders, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}