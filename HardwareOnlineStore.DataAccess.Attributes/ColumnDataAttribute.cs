﻿using System.Data;

namespace HardwareOnlineStore.DataAccess.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class ColumnDataAttribute(string name, DbType dbType) : Attribute
{
    public string Name { get; set; } = name;

    public DbType DbType { get; set; } = dbType;
}