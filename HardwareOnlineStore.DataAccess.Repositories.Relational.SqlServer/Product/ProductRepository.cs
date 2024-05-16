namespace HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Product;

public sealed class ProductRepository(SqlServerProvider<ProductEntity> sqlServer) : IRepository<ProductEntity>
{
    public string DbProviderName => "SqlServer";

    public Task<DbResponse<ProductEntity>> GetByIdAsync(QueryParameters queryParameters, Guid id, CancellationToken token)
       => sqlServer.GetByIdAsync(queryParameters, "id", id, token);

    public Task<DbResponse<ProductEntity>> GetByAsync(QueryParameters queryParameters, ProductEntity productCondition, CancellationToken token)
        => sqlServer.GetByAsync(queryParameters, productCondition, token);

    public Task<DbResponse<ProductEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => sqlServer.SelectAsync(queryParameters, token);

    public Task<DbResponse<ProductEntity>> SelectByAsync(QueryParameters queryParameters, ProductEntity productCondition, CancellationToken token)
        => sqlServer.SelectByAsync(queryParameters, productCondition, token);

    public Task<DbResponse<ProductEntity>> ChangeAsync(QueryParameters queryParameters, ProductEntity product, CancellationToken token)
        => sqlServer.UpdateAsync(queryParameters, product, token);

    public Task<IEnumerable<DbResponse<ProductEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<ProductEntity> products, CancellationToken token)
        => sqlServer.UpdateAsync(queryParameters, products, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}