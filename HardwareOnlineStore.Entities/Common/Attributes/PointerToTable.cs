namespace HardwareOnlineStore.Entities.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class PointerToTable() : Attribute
{
    public required string TableName { get; set; } = string.Empty;
}