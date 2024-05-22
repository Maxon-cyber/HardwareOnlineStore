namespace HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Order;

public sealed class OrderRepository(SqlServerProvider<OrderEntity> sqlServer) : IRepository<OrderEntity>
{
    public string DbProviderName => "SqlServer";

    public Task<DbResponse<OrderEntity>> GetByIdAsync(QueryParameters queryParameters, string? name, Guid? id, CancellationToken token)
       => sqlServer.GetByIdAsync(queryParameters, "id", id, token);

    public Task<DbResponse<OrderEntity>> GetByIdsAsync(QueryParameters queryParameters, string? name, ICollection<Guid>? ids, CancellationToken token)
       => sqlServer.GetByIdsAsync(queryParameters, "id", ids, token);

    public Task<DbResponse<OrderEntity>> GetByAsync(QueryParameters queryParameters, OrderEntity orderCondition, CancellationToken token)
        => sqlServer.GetByAsync(queryParameters, orderCondition, token);

    public Task<DbResponse<OrderEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => sqlServer.SelectAsync(queryParameters, token);

    public Task<DbResponse<OrderEntity>> SelectByAsync(QueryParameters queryParameters, OrderEntity orderCondition, CancellationToken token)
        => sqlServer.SelectByAsync(queryParameters, orderCondition, token);

    public Task<DbResponse<OrderEntity>> ChangeAsync(QueryParameters queryParameters, OrderEntity order, CancellationToken token)
        => sqlServer.UpdateAsync(queryParameters, order, token);

    public Task<IEnumerable<DbResponse<OrderEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<OrderEntity> orders, CancellationToken token)
        => sqlServer.UpdateAsync(queryParameters, orders, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}