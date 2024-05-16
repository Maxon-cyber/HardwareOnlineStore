using HardwareOnlineStore.DataAccess.Providers.Relational.Wrappers.ORM.Implementations.ADO.Sql;
using HardwareOnlineStore.Entities;
using HardwareOnlineStore.Entities.Common.Attributes;
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

    internal static DbParameter AddWithValue(this DbCommand command, string nameOfVariable, string prefix, DbType dbType, object? value = null, ParameterDirection parameterDirection = ParameterDirection.Input, int size = -1)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameOfVariable);
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);

        if (!nameOfVariable.Contains(prefix))
            nameOfVariable = nameOfVariable.Insert(0, prefix);

        DbParameter dbParameter = command.CreateParameter();
        dbParameter.ParameterName = nameOfVariable;
        dbParameter.DbType = dbType;
        dbParameter.Direction = parameterDirection;
        dbParameter.Size = size;

        if (value != null && parameterDirection == ParameterDirection.Input)
            dbParameter.Value = value;

        command.Parameters.Add(dbParameter);

        return dbParameter;
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
        PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

        PropertyInfo[] propertiesWithSetAccessor = properties.Where(p => p.CanWrite).ToArray();

        foreach (PropertyInfo property in propertiesWithSetAccessor)
        {
            object? value = property.GetValue(entity);

            if (value == null)
                continue;

            Type propertyType = property.PropertyType;

            if (value.Equals(propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null))
                continue;

            if (property.GetCustomAttribute<PointerToTable>() != null)
                AddEntityValuesRecursive(command, value, parameterPrefix, ref countOfAddedValues);
            else if (value is IEnumerable<Entity> entities)
            {
                DataTable productTable = new DataTable();
                PropertyInfo[] propertyInfos = entities.First().GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

                PropertyInfo[] entityPropertiesWithSetAccessor = propertyInfos.Where(p => p.CanWrite).ToArray();
                foreach (PropertyInfo propertyInfo in entityPropertiesWithSetAccessor)
                {
                    ColumnDataAttribute? column = propertyInfo.GetCustomAttribute<ColumnDataAttribute>();

                    if (column == null)
                        continue;

                    productTable.Columns.Add($"{parameterPrefix}{column.Name}", propertyInfo.PropertyType);
                }

                foreach (Entity obj in entities)
                {
                    List<object?> values = new List<object?>();

                    foreach (PropertyInfo propertyInfo in propertyInfos.Where(p => p.CanWrite))
                        values.Add(propertyInfo.GetValue(obj));

                    productTable.Rows.Add([.. values]);

                    Interlocked.Increment(ref countOfAddedValues);
                }
                    
                DbParameter productParam = command.CreateParameter();
                productParam.ParameterName = property.GetCustomAttribute<ColumnDataAttribute>().Name;
                productParam.Direction = ParameterDirection.Input;
                productParam.Value = productTable;

                command.Parameters.Add(productParam);
            }
            else
            {
                ColumnDataAttribute? columnDataAttribute = property.GetCustomAttribute<ColumnDataAttribute>();

                if (columnDataAttribute == null)
                    continue;

                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = @$"{parameterPrefix}{columnDataAttribute.Name}";
                parameter.DbType = columnDataAttribute.DbType;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = value;

                command.Parameters.Add(parameter);

                Interlocked.Increment(ref countOfAddedValues);
            }
        }
    }

    internal static async Task<TEntity> MappingAsync<TEntity>(this DbDataReader dbDataReader)
        where TEntity : Entity
    {
        TEntity entity = Activator.CreateInstance<TEntity>();

        await Task.Run(() =>
        {
            Dictionary<string, object> columnNamesAndValues = [];

            for (int index = 0; index < dbDataReader.VisibleFieldCount; index++)
                columnNamesAndValues.Add(dbDataReader.GetName(index), dbDataReader.GetValue(index));

            MappingRecursive(entity, columnNamesAndValues);
        });

        return entity;
    }

    private static void MappingRecursive(object entity, Dictionary<string, object> columnNamesAndValues)
    {
        PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (PropertyInfo property in properties)
        {
            ColumnDataAttribute? attribute = property.GetCustomAttribute<ColumnDataAttribute>();

            Type propertyType = property.PropertyType;

            if (attribute != null)
                if (columnNamesAndValues.TryGetValue(attribute.Name, out object? value))
                {
                    if (property.GetSetMethod() == null)
                    {
                        FieldInfo? backingField = property.DeclaringType.GetField($"<{property.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                        backingField.SetValue(entity, SqlHelper.ConvertToCLRType(value, propertyType));
                    }
                    else
                        property.SetValue(entity, SqlHelper.ConvertToCLRType(value, propertyType));

                    continue;
                }

            if (propertyType.IsClass && propertyType != typeof(string) && !propertyType.IsArray && property.GetCustomAttribute<PointerToTable>() != null)
            {
                object nestedEntity = Activator.CreateInstance(propertyType)!;

                MappingRecursive(nestedEntity, columnNamesAndValues);

                property.SetValue(entity, nestedEntity);

                continue;
            }

            else if (typeof(IEnumerable<>).IsAssignableFrom(property.PropertyType))
            {
                Type? elementType = property.PropertyType.IsArray
                                   ? property.PropertyType.GetElementType()
                                   : property.PropertyType.GetGenericArguments().FirstOrDefault();

                if (elementType != null)
                {
                    IList<object> collection = (IList<object>)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                    object collectionParameter = Activator.CreateInstance(elementType);
                    MappingRecursive(collectionParameter, columnNamesAndValues);
                    collection.Add(collectionParameter);

                    property.SetValue(entity, collection);
                }

                continue;
            }
        }
    }
}