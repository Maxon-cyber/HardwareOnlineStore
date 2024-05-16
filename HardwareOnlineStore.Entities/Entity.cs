using HardwareOnlineStore.Entities.Common.Attributes;
using System.Data;

namespace HardwareOnlineStore.Entities;

public abstract class Entity()
{
    [ColumnData(Name = "id", DbType = DbType.Guid)]
    public Guid Id { get; }

    [ColumnData(Name = "time_created", DbType = DbType.DateTime2)]
    public DateTime TimeCreated { get; }

    [ColumnData(Name = "last_access_time", DbType = DbType.DateTime2)]
    public DateTime LastAccessTime { get; }

    [ColumnData(Name = "last_update_time", DbType = DbType.DateTime2)]
    public DateTime LastUpdateTime { get; }
}