using System.Collections.Immutable;

namespace HardwareOnlineStore.DataAccess.Providers.Relational.Models;

public sealed class DbResponse<TType>()
{
    public Queue<TType> QueryResult { get; } = [];

    public Dictionary<object, object> AdditionalData { get; } = new Dictionary<object, object>();

    public string Message { get; set; } = null;

    public Exception? Error { get; set; } = null;

    public object? OutputValue { get; set; } = null;

    public object? ReturnedValue { get; set; } = null;
}