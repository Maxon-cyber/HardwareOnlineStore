﻿using HardwareOnlineStore.Entities.Common.Attributes;
using System.Data;

namespace HardwareOnlineStore.Entities.User;

public sealed class Location() : Entity
{
    [ColumnData(ColumnName = "house_number", DbType = DbType.String)]
    public string HouseNumber { get; set; }

    [ColumnData(ColumnName = "street", DbType = DbType.String)]
    public string Street { get; set; }

    [ColumnData(ColumnName = "city", DbType = DbType.String)]
    public string City { get; init; }

    [ColumnData(ColumnName = "region", DbType = DbType.String)]
    public string Region { get; init; }

    [ColumnData(ColumnName = "country", DbType = DbType.String)]
    public string Country { get; init; }
}