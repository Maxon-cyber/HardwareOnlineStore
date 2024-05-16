using HardwareOnlineStore.Entities.Common.Attributes;
using System.Data;

namespace HardwareOnlineStore.Entities.User;

public sealed class UserEntity() : Entity
{
    [ColumnData(Name = "name", DbType = DbType.String)]
    public string Name { get; init; }

    [ColumnData(Name = "second_name", DbType = DbType.String)]
    public string SecondName { get; init; }

    [ColumnData(Name = "patronymic", DbType = DbType.String)]
    public string Patronymic { get; init; }

    [ColumnData(Name = "gender", DbType = DbType.String)]
    public Gender Gender { get; init; }

    [ColumnData(Name = "age", DbType = DbType.Int32)]
    public uint Age { get; init; }

    [ColumnData(Name = "login", DbType = DbType.String)]
    public string Login { get; init; }

    [ColumnData(Name = "password", DbType = DbType.Binary)]
    public byte[] Password { get; init; }

    [ColumnData(Name = "role", DbType = DbType.String)]
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

public sealed class Location() : Entity
{
    [ColumnData(Name = "house_number", DbType = DbType.String)]
    public string HouseNumber { get; set; }

    [ColumnData(Name = "street", DbType = DbType.String)]
    public string Street { get; set; }

    [ColumnData(Name = "city", DbType = DbType.String)]
    public string City { get; init; }

    [ColumnData(Name = "region", DbType = DbType.String)]
    public string Region { get; init; }

    [ColumnData(Name = "country", DbType = DbType.String)]
    public string Country { get; init; }
}