using HardwareOnlineStore.Entities.Common.Attributes;
using System.Data;

namespace HardwareOnlineStore.Entities.User;

public sealed class UserEntity() : Entity
{
    [ColumnData(ColumnName = "name", DbType = DbType.String)]
    public string Name { get; init; }

    [ColumnData(ColumnName = "second_name", DbType = DbType.String)]
    public string SecondName { get; init; }

    [ColumnData(ColumnName = "patronymic", DbType = DbType.String)]
    public string Patronymic { get; init; }

    [ColumnData(ColumnName = "gender", DbType = DbType.String)]
    public Gender Gender { get; init; }

    [ColumnData(ColumnName = "age", DbType = DbType.Int32)]
    public uint Age { get; init; }

    [ColumnData(ColumnName = "login", DbType = DbType.String)]
    public string Login { get; init; }

    [ColumnData(ColumnName = "password", DbType = DbType.Binary)]
    public byte[] Password { get; init; }

    [ColumnData(ColumnName = "role", DbType = DbType.String)]
    public Role Role { get; init; }

    [PointerToTable(TableName = "UserLocation")]
    public Location Location { get; init; }

    public override string ToString()
       => $"{Name} {SecondName} {Patronymic}";

    public override bool Equals(object? obj)
        => base.Equals(obj);

    public override int GetHashCode()
        => base.GetHashCode();
}