using HardwareOnlineStore.DataAccess.Providers.Relational.Implementations.SqlServer;
using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Repositories.Relational.Abstractions;
using HardwareOnlineStore.Entities.User;

namespace HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.User;

public sealed class UserRepository(SqlServerProvider<UserEntity> sqlServer) : IRepository<UserEntity>
{
    public string DbProviderName => "SqlServer";

    public Task<DbResponse<UserEntity>> GetByAsync(QueryParameters queryParameters, UserEntity userCondition, CancellationToken token)
        => sqlServer.GetValueByAsync(queryParameters, userCondition, token);

    public Task<DbResponse<UserEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => sqlServer.SelectValuesAsync(queryParameters, token);

    public Task<DbResponse<UserEntity>> SelectByAsync(QueryParameters queryParameters, UserEntity userCondition, CancellationToken token)
        => sqlServer.SelectValuesByAsync(queryParameters, userCondition, token);

    public Task<DbResponse<UserEntity>> ChangeAsync(QueryParameters queryParameters, UserEntity user, CancellationToken token)
        => sqlServer.ChangeValueAsync(queryParameters, user, token);

    public Task<IEnumerable<DbResponse<UserEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<UserEntity> users, CancellationToken token)
        => sqlServer.ChangeValuesAsync(queryParameters, users, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}