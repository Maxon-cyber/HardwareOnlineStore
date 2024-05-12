using HardwareOnlineStore.DataAccess.Providers.Relational.Implementations.SqlServer;
using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Repositories.Relational.Abstractions;
using HardwareOnlineStore.Entities.Product;

namespace HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Product;

public sealed class ProductRepository(SqlServerProvider<ProductEntity> sqlServer) : IRepository<ProductEntity>
{
    public string DbProviderName => "SqlServer";

    public Task<DbResponse<ProductEntity>> GetByAsync(QueryParameters queryParameters, ProductEntity productCondition, CancellationToken token)
        => sqlServer.GetValueByAsync(queryParameters, productCondition, token);

    public Task<DbResponse<ProductEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => sqlServer.SelectValuesAsync(queryParameters, token);

    public Task<DbResponse<ProductEntity>> SelectByAsync(QueryParameters queryParameters, ProductEntity productCondition, CancellationToken token)
        => sqlServer.SelectValuesByAsync(queryParameters, productCondition, token);

    public Task<DbResponse<ProductEntity>> ChangeAsync(QueryParameters queryParameters, ProductEntity product, CancellationToken token)
        => sqlServer.ChangeValueAsync(queryParameters, product, token);

    public Task<IEnumerable<DbResponse<ProductEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<ProductEntity> products, CancellationToken token)
        => sqlServer.ChangeValuesAsync(queryParameters, products, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}