using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using System.Data.Common;

namespace HardwareOnlineStore.DataAccess.Providers.Relational.Abstractions;

public abstract class DbProvider<TParameter>(ConnectionParameters connectionParameters) : IDisposable, IAsyncDisposable
{
    protected readonly ConnectionParameters _connectionParameters = connectionParameters;

    protected DbConnection _dbConnection;
    protected DbCommand _dbCommand;

    public abstract string Prefix { get; }

    public abstract string Provider { get; }

    public abstract DbConnection DbConnection { get; }

    public abstract DbCommand DbCommand { get; }

    public abstract Task<DbResponse<TParameter>> GetValueByAsync(QueryParameters queryParameters, TParameter parameterCondition, CancellationToken token);

    public abstract Task<DbResponse<TParameter>> SelectValuesAsync(QueryParameters queryParameters, CancellationToken token);

    public abstract Task<DbResponse<TParameter>> SelectValuesByAsync(QueryParameters queryParameters, TParameter parameterCondition, CancellationToken token);

    public abstract Task<DbResponse<TParameter>> ChangeValueAsync(QueryParameters queryParameters, TParameter parameter, CancellationToken token);

    public abstract Task<IEnumerable<DbResponse<TParameter>>> ChangeValuesAsync(QueryParameters queryParameters, IEnumerable<TParameter> parameters, CancellationToken token);

    public abstract void Dispose();

    public abstract ValueTask DisposeAsync();
}