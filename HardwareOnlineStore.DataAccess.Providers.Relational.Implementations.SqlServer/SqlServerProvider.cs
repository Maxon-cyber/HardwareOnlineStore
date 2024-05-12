using HardwareOnlineStore.DataAccess.Providers.Relational.Abstractions;
using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.Entities;
using HardwareStore.Providers.Relational.DataTools.ADO;
using Microsoft.Data.SqlClient;

namespace HardwareOnlineStore.DataAccess.Providers.Relational.Implementations.SqlServer;

public sealed class SqlServerProvider<TEntity> : DbProvider<TEntity>
    where TEntity : Entity
{
    private readonly ADOEntityDataAccessService<TEntity> _ado;

    public SqlServerProvider(ConnectionParameters connectionParameters) : base(connectionParameters)
        => _ado = new ADOEntityDataAccessService<TEntity>(Provider, Prefix, DbConnection, DbCommand);

    public override string Prefix => "@";

    public override string Provider => "SqlServerProvider";

    public override SqlConnection DbConnection
    {
        get
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder
            {
                DataSource = $"{_connectionParameters.Server}",
                InitialCatalog = _connectionParameters.Database,
                IntegratedSecurity = _connectionParameters.IntegratedSecurity
            };

            if (!_connectionParameters.IntegratedSecurity)
            {
                sqlConnectionBuilder.UserID = _connectionParameters.Username;
                sqlConnectionBuilder.Password = _connectionParameters.Password;
            }

            sqlConnectionBuilder.TrustServerCertificate = _connectionParameters.TrustServerCertificate;

            if (_connectionParameters.ConnectionTimeout.HasValue)
                sqlConnectionBuilder.ConnectTimeout = (int)_connectionParameters.ConnectionTimeout.Value.TotalSeconds;
            if (_connectionParameters.MaxPoolSize.HasValue)
                sqlConnectionBuilder.MaxPoolSize = _connectionParameters.MaxPoolSize.Value;

            _dbConnection = new SqlConnection(sqlConnectionBuilder.ToString());

            return (_dbConnection as SqlConnection)!;
        }
    }

    public override SqlCommand DbCommand
    {
        get
        {
            if (_dbConnection == null)
                throw new Exception();

            _dbCommand = new SqlCommand()
            {
                Connection = _dbConnection as SqlConnection,
                CommandTimeout = 30
            };

            return (_dbCommand as SqlCommand)!;
        }
    }

    public async override Task<DbResponse<TEntity>> GetValueByAsync(QueryParameters queryParameters, TEntity entityCondition, CancellationToken token)
        => await _ado.GetEntityByAsync(queryParameters, entityCondition, token);

    public async override Task<DbResponse<TEntity>> SelectValuesAsync(QueryParameters queryParameters, CancellationToken token)
        => await _ado.SelectEntitiesAsync(queryParameters, token);

    public async override Task<DbResponse<TEntity>> SelectValuesByAsync(QueryParameters queryParameters, TEntity entityCondition, CancellationToken token)
        => await _ado.SelectEntitiesByAsync(queryParameters, entityCondition, token);

    public async override Task<DbResponse<TEntity>> ChangeValueAsync(QueryParameters queryParameters, TEntity entity, CancellationToken token)
        => await _ado.UpdateEntityAsync(queryParameters, entity, token);

    public async override Task<IEnumerable<DbResponse<TEntity>>> ChangeValuesAsync(QueryParameters queryParameters, IEnumerable<TEntity> entities, CancellationToken token)
        => await _ado.ChangeEntityAsync(queryParameters, entities, token);

    public override void Dispose()
        => _ado.Dispose();

    public override async ValueTask DisposeAsync()
        => await _ado.DisposeAsync();
}