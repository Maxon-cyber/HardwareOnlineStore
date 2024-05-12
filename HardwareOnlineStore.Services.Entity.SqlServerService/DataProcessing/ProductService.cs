using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Product;
using HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Queries;
using HardwareOnlineStore.Entities.Product;
using HardwareOnlineStore.Services.Entity.Contracts;
using HardwareOnlineStore.Services.Entity.Contracts.Abstractions;
using HardwareOnlineStore.Services.Utilities.Logger.File;
using System.Collections.Immutable;
using System.Data;

namespace HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;

public sealed class ProductService(ProductRepository repository, FileLogger logger) : EntityService<ProductEntity>(repository, logger)
{
    public async Task<ProductEntity?> GetProductAsync(ProductEntity productCondition)
    {
        ProductEntity? product = await GetByAsync(productCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetProductByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
        });

        return product;
    }

    public async Task<IEnumerable<ProductEntity>?> SelectProductsAsync()
    {
        IEnumerable<ProductEntity>? products = await SelectAsync(new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetAllProducts,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return products;
    }

    public async Task<IEnumerable<ProductEntity>?> SelectProductsByAsync(ProductEntity productCondition)
    {
        IEnumerable<ProductEntity>? products = await SelectByAsync(productCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetAllProductsByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return products;
    }

    public async Task<object?> ChangeProductAsync(TypeOfUpdateCommand typeOfCommand, ProductEntity product)
    {
        string command = typeOfCommand switch
        {
            TypeOfUpdateCommand.Insert => SqlServerStoredProcedureList.AddProduct,
            TypeOfUpdateCommand.Update => SqlServerStoredProcedureList.UpadateProduct,
            TypeOfUpdateCommand.Delete => SqlServerStoredProcedureList.DropProduct,
            _ => throw new NotImplementedException(),
        };

        object? result = await ChangeAsync(product, new QueryParameters()
        {
            CommandText = command,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
            OutputParameter = new Parameter()
            {
                Name = "@result",
                DbType = DbType.String,
                Size = -1,
                ParameterDirection = ParameterDirection.Output
            },
            ReturnedValue = new Parameter()
            {
                Name = "@returned_value",
                DbType = DbType.Int32,
                ParameterDirection = ParameterDirection.ReturnValue
            }
        });

        return result;
    }

    public async Task<ImmutableDictionary<string, object?>> ChangeProductAsync(TypeOfUpdateCommand typeOfCommand, IEnumerable<ProductEntity> products)
    {
        string command = typeOfCommand switch
        {
            TypeOfUpdateCommand.Insert => SqlServerStoredProcedureList.AddProduct,
            TypeOfUpdateCommand.Update => SqlServerStoredProcedureList.UpadateProduct,
            TypeOfUpdateCommand.Delete => SqlServerStoredProcedureList.DropProduct,
            _ => throw new NotImplementedException(),
        };

        ImmutableDictionary<string, object?> result = await ChangeAsync(products, new QueryParameters()
        {
            CommandText = command,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
            OutputParameter = new Parameter()
            {
                Name = "@result",
                Size = -1,
                DbType = DbType.Int32,
                ParameterDirection = ParameterDirection.Output
            },
            ReturnedValue = new Parameter()
            {
                Name = "@returned_value",
                DbType = DbType.Int32,
                ParameterDirection = ParameterDirection.ReturnValue
            }
        });
       
        return result;
    }

    public new void Dispose()
        => base.Dispose();

    public new async ValueTask DisposeAsync()
        => await base.DisposeAsync();
}