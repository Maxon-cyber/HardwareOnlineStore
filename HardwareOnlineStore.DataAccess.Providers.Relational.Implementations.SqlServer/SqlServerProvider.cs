using HardwareOnlineStore.DataAccess.Providers.Relational.Abstractions;
using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Implementations.ADO;
using HardwareOnlineStore.Entities;
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
            if (_dbConnection != null)
                return (_dbConnection as SqlConnection)!;

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
            ArgumentNullException.ThrowIfNull(_dbConnection);

            _dbCommand = new SqlCommand()
            {
                Connection = _dbConnection as SqlConnection,
                CommandTimeout = 30
            };

            return (_dbCommand as SqlCommand)!;
        }
    }

    public async override Task<DbResponse<TEntity>> GetByIdAsync(QueryParameters queryParameters, string columnName, Guid id, CancellationToken token)
        => await _ado.GetByIdAsync(queryParameters, columnName, id, token);

    public async override Task<DbResponse<TEntity>> GetByAsync(QueryParameters queryParameters, TEntity entityCondition, CancellationToken token)
        => await _ado.GetByAsync(queryParameters, entityCondition, token);

    public async override Task<DbResponse<TEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => await _ado.SelectAsync(queryParameters, token);

    public async override Task<DbResponse<TEntity>> SelectByAsync(QueryParameters queryParameters, TEntity entityCondition, CancellationToken token)
        => await _ado.SelectByAsync(queryParameters, entityCondition, token);

    public async override Task<DbResponse<TEntity>> UpdateAsync(QueryParameters queryParameters, TEntity entity, CancellationToken token)
        => await _ado.UpdateAsync(queryParameters, entity, token);

    public async override Task<IEnumerable<DbResponse<TEntity>>> UpdateAsync(QueryParameters queryParameters, IEnumerable<TEntity> entities, CancellationToken token)
        => await _ado.UpdateAsync(queryParameters, entities, token);

    public override void Dispose()
        => _ado.Dispose();

    public override async ValueTask DisposeAsync()
        => await _ado.DisposeAsync();
}