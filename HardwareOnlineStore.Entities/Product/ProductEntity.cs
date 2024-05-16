using HardwareOnlineStore.Entities.Common.Attributes;
using System.Data;

namespace HardwareOnlineStore.Entities.Product;

public sealed class ProductEntity() : Entity
{
    [ColumnData(Name = "name", DbType = DbType.String)]
    public string Name { get; init; }

    [ColumnData(Name = "image", DbType = DbType.Binary)]
    public byte[] Image { get; init; }

    [ColumnData(Name = "quantity", DbType = DbType.Int32)]
    public int Quantity { get; init; }

    [ColumnData(Name = "category", DbType = DbType.String)]
    public string Category { get; init; }

    [ColumnData(Name = "price", DbType = DbType.Decimal)]
    public decimal Price { get; init; }

    public string ToString(string message)
        => $"{Name}-{Price}({message})";

    public override string ToString()
        => $"{Name}-{Price}";

    public override bool Equals(object? obj)
        => base.Equals(obj);

    public override int GetHashCode()
        => base.GetHashCode();
}