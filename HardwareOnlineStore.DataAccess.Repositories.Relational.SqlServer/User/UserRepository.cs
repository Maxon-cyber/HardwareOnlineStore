
namespace HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.User;

public sealed class UserRepository(SqlServerProvider<UserEntity> sqlServer) : IRepository<UserEntity>
{
    public string DbProviderName => "SqlServer";

    public Task<DbResponse<UserEntity>> GetByIdAsync(QueryParameters queryParameters, Guid id, CancellationToken token)
        => sqlServer.GetByIdAsync(queryParameters, "id", id, token);

    public Task<DbResponse<UserEntity>> GetByAsync(QueryParameters queryParameters, UserEntity userCondition, CancellationToken token)
        => sqlServer.GetByAsync(queryParameters, userCondition, token);

    public Task<DbResponse<UserEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => sqlServer.SelectAsync(queryParameters, token);

    public Task<DbResponse<UserEntity>> SelectByAsync(QueryParameters queryParameters, UserEntity userCondition, CancellationToken token)
        => sqlServer.SelectByAsync(queryParameters, userCondition, token);

    public Task<DbResponse<UserEntity>> ChangeAsync(QueryParameters queryParameters, UserEntity user, CancellationToken token)
        => sqlServer.UpdateAsync(queryParameters, user, token);

    public Task<IEnumerable<DbResponse<UserEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<UserEntity> users, CancellationToken token)
        => sqlServer.UpdateAsync(queryParameters, users, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}