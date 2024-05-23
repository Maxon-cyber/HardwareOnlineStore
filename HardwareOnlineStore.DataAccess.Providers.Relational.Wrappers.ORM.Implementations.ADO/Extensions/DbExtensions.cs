﻿using HardwareOnlineStore.DataAccess.Attributes;
using HardwareOnlineStore.DataAccess.Providers.Relational.Models;
using HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Implementations.ADO.Sql;
using HardwareOnlineStore.Entities;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Implementations.ADO.Extensions;

internal static class DbExtensions
{
    internal static async Task OpenConnectionAsync(this DbConnection dbConnection, CancellationToken token)
    {
        if (dbConnection.State == ConnectionState.Closed)
            await dbConnection.OpenAsync(token);
    }

    internal static async Task CloseConnectionAsync(this DbConnection dbConnection)
    {
        if (dbConnection.State == ConnectionState.Open)
            await dbConnection.CloseAsync();
    }

    internal static async Task DisposeAndRollbackTransactionAsync(this DbTransaction? transaction, CancellationToken token)
    {
        if (transaction != null)
        {
            await transaction.RollbackAsync(token);
            await transaction.DisposeAsync();
        }
    }

    internal static async Task CommitTransactionAsync(this DbTransaction? transaction, CancellationToken token)
    {
        if (transaction != null)
            await transaction.CommitAsync(token);
    }

    internal static int AddValue(this DbCommand command, object? value, string prefix, string? name, out DbParameter dbParameter)
    {
        dbParameter = null!;

        if (value == null)
            return command.Parameters.Count;

        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);

        dbParameter = command.CreateParameter();

        if (name != null)
            if (!name.Contains(prefix))
                dbParameter.ParameterName = name.Insert(0, prefix);

        dbParameter.DbType = SqlHelper.ConvertToDbType(value);
        dbParameter.Direction = ParameterDirection.Input;
        dbParameter.Size = -1;
        dbParameter.Value = value;

        command.Parameters.Add(dbParameter);

        return command.Parameters.Count;
    }

    internal static int AddParameter(this DbCommand command, Parameter? parameter, string prefix, out DbParameter dbParameter)
    {
        dbParameter = null!;

        if (parameter == null)
            return command.Parameters.Count;

        ArgumentException.ThrowIfNullOrWhiteSpace(parameter.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);

        string name = string.Empty;

        if (!parameter.Name.Contains(prefix))
            name = parameter.Name.Insert(0, prefix);

        dbParameter = command.CreateParameter();
        dbParameter.ParameterName = name;
        dbParameter.Direction = parameter.ParameterDirection;
        dbParameter.Size = -1;

        if (parameter.Value != null)
        {
            dbParameter.Value = parameter.Value;
            dbParameter.DbType = parameter.DbType;
        }

        command.Parameters.Add(dbParameter);

        return command.Parameters.Count;
    }

    internal static async Task<int> AddEntityValuesAsync<TEntity>(this DbCommand command, TEntity entity, string parameterPrefix)
        where TEntity : Entity
    {
        ArgumentNullException.ThrowIfNull(entity);

        int countOfAddedValues = 0;

        await Task.Run(() => AddEntityValuesRecursive(command, entity, parameterPrefix, ref countOfAddedValues));

        return countOfAddedValues;
    }

    private static void AddEntityValuesRecursive(DbCommand command, object entity, string parameterPrefix, ref int countOfAddedValues)
    {
        IEnumerable<PropertyInfo> propertiesWithSetAccessor = entity.GetType()
                                          .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          .Where(p => p.CanWrite);

        foreach (PropertyInfo property in propertiesWithSetAccessor)
        {
            object? value = property.GetValue(entity);

            if (value == null)
                continue;

            Type propertyType = property.PropertyType;

            if (value.Equals(propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null))
                continue;

            bool isCollection = typeof(IEnumerable).IsAssignableFrom(property.PropertyType);

            bool isColumn = property.GetCustomAttribute<ColumnDataAttribute>() != null;

            if (property.GetCustomAttribute<PointerToTable>() != null && isColumn)
                AddEntityValuesRecursive(command, value, parameterPrefix, ref countOfAddedValues);
            if (isCollection && isColumn)
            {
                DataTable table = new DataTable();

                IEnumerable entities = (IEnumerable)value;

                Type? elementType = propertyType.IsArray
                       ? propertyType.GetElementType()
                       : propertyType.GetGenericArguments().FirstOrDefault();

                if (elementType != null)
                    if (!elementType.IsClass)
                        continue;

                IEnumerable<PropertyInfo> entityPropertiesWithSetAccessor = entities.Cast<object>()
                                                       .First()
                                                       .GetType()
                                                       .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                                                       .Where(p => p.CanWrite);

                foreach (PropertyInfo propertyInfo in entityPropertiesWithSetAccessor)
                {
                    SqlParameterAttribute? column = propertyInfo.GetCustomAttribute<SqlParameterAttribute>();

                    if (column == null)
                        continue;

                    table.Columns.Add($"{parameterPrefix}{column.ParameterName}", propertyInfo.PropertyType);
                }

                foreach (object obj in entities)
                {
                    List<object?> values = [];

                    foreach (PropertyInfo propertyInfo in entityPropertiesWithSetAccessor.Where(p => p.CanWrite))
                        values.Add(propertyInfo.GetValue(obj));

                    table.Rows.Add([.. values]);

                    Interlocked.Increment(ref countOfAddedValues);
                }

                DbParameter productParam = command.CreateParameter();
                productParam.ParameterName = property.GetCustomAttribute<SqlParameterAttribute>()!.ParameterName;
                productParam.Direction = ParameterDirection.Input;
                productParam.Value = table;

                command.Parameters.Add(productParam);
            }
            else
            {
                SqlParameterAttribute? sqlParameter = property.GetCustomAttribute<SqlParameterAttribute>();

                if (sqlParameter == null)
                    continue;

                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = @$"{parameterPrefix}{sqlParameter.ParameterName}";
                parameter.DbType = sqlParameter.DbType;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = value;

                command.Parameters.Add(parameter);

                Interlocked.Increment(ref countOfAddedValues);
            }
        }
    }

    internal static async Task<List<TEntity>> MappingAsync<TEntity>(this DbDataReader dbDataReader, CancellationToken token)
        where TEntity : Entity, new()
    {
        Dictionary<string, TEntity> entities = [];

        while (await dbDataReader.ReadAsync(token))
        {
            try
            {
                string id = dbDataReader["id"].ToString();

                if (!entities.TryGetValue(id, out TEntity? value))
                {
                    value = new TEntity();
                    entities[id] = value;
                }

                TEntity currentEntity = value;

                List<Tuple<string, object>> columnNamesAndValues = [];

                for (int index = 0; index < dbDataReader.FieldCount; index++)
                    columnNamesAndValues.Add(new Tuple<string, object>(dbDataReader.GetName(index), dbDataReader.GetValue(index)));

                MappingRecursive(currentEntity, columnNamesAndValues);
            }
            catch (IndexOutOfRangeException)
            {
                continue;
            }
        }

        return [.. entities.Values];
    }

    private static void MappingRecursive(object entity, List<Tuple<string, object>> columnNamesAndValues)
    {
        PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (PropertyInfo property in properties)
        {
            SqlParameterAttribute? attribute = property.GetCustomAttribute<SqlParameterAttribute>();

            Type propertyType = property.PropertyType;

            if (columnNamesAndValues.Any(t => t.Item1 == attribute?.ParameterName))
            {
                object value = columnNamesAndValues.FirstOrDefault(t => t.Item1 == attribute?.ParameterName)?.Item2!;
                object convertedValue = SqlHelper.ConvertToCLRType(value, propertyType);

                if (!property.CanWrite)
                {
                    FieldInfo? backingField = property.DeclaringType?.GetField($"<{property.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                    backingField?.SetValue(entity, convertedValue);
                }
                else
                    property.SetValue(entity, convertedValue);

                continue;
            }

            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                Type? elementType = property.PropertyType.IsArray
                       ? property.PropertyType.GetElementType()
                       : property.PropertyType.GetGenericArguments().FirstOrDefault();

                if (elementType != null)
                {
                    IList collection = (IList)property.GetValue(entity) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;

                    object collectionParameter = Activator.CreateInstance(elementType)!;
                    MappingRecursive(collectionParameter, columnNamesAndValues);

                    collection.Add(collectionParameter);
                    property.SetValue(entity, collection);
                }

                continue;
            }
            else if (property.GetCustomAttribute<PointerToTable>() != null)
            {
                object nestedEntity = Activator.CreateInstance(propertyType)!;

                MappingRecursive(nestedEntity, columnNamesAndValues);

                property.SetValue(entity, nestedEntity);

                continue;
            }
        }
    }
}