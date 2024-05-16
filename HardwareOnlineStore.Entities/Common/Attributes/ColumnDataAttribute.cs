using System.Data;

namespace HardwareOnlineStore.Entities.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class ColumnDataAttribute() : Attribute
{
    public required string Name { get; set; } = string.Empty;

    public required DbType DbType { get; set; } = DbType.String;
}