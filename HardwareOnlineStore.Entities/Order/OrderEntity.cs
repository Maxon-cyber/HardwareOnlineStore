using HardwareOnlineStore.Entities.Common.Attributes;
using System.Data;

namespace HardwareOnlineStore.Entities.Order;

public sealed class OrderEntity() : Entity
{
    [ColumnData(Name = "user_id", DbType = DbType.Guid)]
    public Guid UserId { get; init; }

    [ColumnData(Name = "order_details_table", DbType = DbType.Object)]
    public ICollection<OrderItem> Items { get; init; }

    [ColumnData(Name = "total_amount", DbType = DbType.Decimal)]
    public decimal TotalAmount { get; init; }

    [ColumnData(Name = "delivery_date", DbType = DbType.DateTime2)]
    public DateTime DeliveryDate { get; init; }

    [ColumnData(Name = "status", DbType = DbType.String)]
    public Status Status { get; init; }
}

public sealed class OrderItem() : Entity
{
    [ColumnData(Name = "product_id", DbType = DbType.Guid)]
    public Guid ProductId { get; init; }

    [ColumnData(Name = "number_of_products", DbType = DbType.Int32)]
    public int NumberOfProducts { get; init; }
}