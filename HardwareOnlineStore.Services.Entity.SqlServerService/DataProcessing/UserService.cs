﻿using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.Queries;
using HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer.User;
using HardwareOnlineStore.Entities.User;
using HardwareOnlineStore.Services.Entity.Contracts;
using HardwareOnlineStore.Services.Entity.Contracts.Abstractions;
using HardwareOnlineStore.Services.Utilities.Logger.File;
using System.Collections.Immutable;
using System.Data;

namespace HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;

public sealed class UserService(UserRepository userRepository, FileLogger logger) : EntityService<UserEntity>(userRepository, logger)
{
    public async Task<UserEntity?> GetUserAsync(UserEntity userCondition)
    {
        UserEntity? user = await GetByAsync(userCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetUserByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
        });

        return user;
    }

    public async Task<IEnumerable<UserEntity>?> SelectUsersAsync()
    {
        IEnumerable<UserEntity>? users = await SelectAsync(new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetAllUsers,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return users;
    }

    public async Task<IEnumerable<UserEntity>?> SelectUsersByAsync(UserEntity userCondition)
    {
        IEnumerable<UserEntity>? users = await SelectByAsync(userCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetUsersByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return users;
    }

    public async Task<object?> ChangeUserAsync(TypeOfUpdateCommand typeOfCommand, UserEntity user)
    {
        string command = typeOfCommand switch
        {
            TypeOfUpdateCommand.Insert => SqlServerStoredProcedureList.AddUser,
            TypeOfUpdateCommand.Update => SqlServerStoredProcedureList.UpadateUser,
            TypeOfUpdateCommand.Delete => SqlServerStoredProcedureList.DropUser,
            _ => throw new NotImplementedException(),
        };

        object? result = await ChangeAsync(user, new QueryParameters()
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
                DbType = DbType.Boolean,
                ParameterDirection = ParameterDirection.ReturnValue
            }
        });

        return result;
    }

    public async Task<ImmutableDictionary<string, object?>> ChangeUserAsync(TypeOfUpdateCommand typeOfCommand, IEnumerable<UserEntity> users)
    {
        string command = typeOfCommand switch
        {
            TypeOfUpdateCommand.Insert => SqlServerStoredProcedureList.AddUser,
            TypeOfUpdateCommand.Update => SqlServerStoredProcedureList.AddUser,
            TypeOfUpdateCommand.Delete => SqlServerStoredProcedureList.DropUser,
            _ => throw new NotImplementedException(),
        };

        ImmutableDictionary<string, object?> result = await ChangeAsync(users, new QueryParameters()
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
                DbType = DbType.Boolean,
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