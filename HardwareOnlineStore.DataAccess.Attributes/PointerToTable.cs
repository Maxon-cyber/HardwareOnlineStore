namespace HardwareOnlineStore.DataAccess.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class PointerToTable(string tableName) : Attribute
{
    public string TableName { get; } = tableName;
}