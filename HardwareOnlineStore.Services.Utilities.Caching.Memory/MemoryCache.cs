using HardwareOnlineStore.Services.Utilities.Caching.Abstractions;
using System.Collections.Immutable;
using System.Formats.Tar;

namespace HardwareOnlineStore.Services.Utilities.Caching.Memory;

file class CacheStorage
{
    public static Dictionary<string, object> Cache { get; }

    static CacheStorage()
        => Cache = new Dictionary<string, object>();
}

public sealed class MemoryCache<TValue> : ICache<string, TValue>
    where TValue : notnull
{
    private uint _subscriberCount;

    private static readonly Lazy<MemoryCache<TValue>> _lazyInstance = new Lazy<MemoryCache<TValue>>(() => new MemoryCache<TValue>());
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 20);

    private event EventHandler<CacheChangedEventArgs<string, TValue>> Cache_Changed;

    public static MemoryCache<TValue> Instance => _lazyInstance.Value;

    private MemoryCache() { }

    public event EventHandler<CacheChangedEventArgs<string, TValue>>? CacheChanged
    {
        add
        {
            Cache_Changed += value;
            _subscriberCount++;
        }
        remove
        {
            if (_subscriberCount == 0)
                return;

            Cache_Changed -= value;
            _subscriberCount--;
        }
    }

    public bool IsEmpty => CacheStorage.Cache.Count == 0;

    public IEnumerable<TType>? Of<TType>()
        where TType : notnull
    {
        IEnumerable<KeyValuePair<string, object>>? findDataOfType = CacheStorage.Cache.Where(kvp => kvp.Value is TType);

        IEnumerable<TType>? result = findDataOfType?.Select(kvp => (TType)kvp.Value);

        return result;
    }

    public TType? Of<TType>(Func<TType, bool> func)
        where TType : notnull
    {
        IEnumerable<KeyValuePair<string, object>>? findDataOfType = CacheStorage.Cache.Where(kvp => kvp.Value is TType);

        IEnumerable<TType>? result = findDataOfType?.Select(kvp => (TType)kvp.Value).Where(func);

        if (result == null)
            return default;

        return result.FirstOrDefault();
    }

    public async Task ClearAsync()
    {
        await _semaphore.WaitAsync();

        CacheStorage.Cache.Clear();

        _semaphore.Release();
    }

    public async Task<bool> ContainsKeyAsync(string key)
    {
        await _semaphore.WaitAsync();

        bool isContains = await Task.FromResult(CacheStorage.Cache.ContainsKey(key));

        _semaphore.Release();

        return isContains;
    }

    public async Task<IImmutableDictionary<string, TValue>?> ReadAsync()
    {
        await _semaphore.WaitAsync();

        ImmutableDictionary<string, TValue> cache = CacheStorage.Cache
                                                                .Where(kvp => kvp.Value is TValue) 
                                                                .ToImmutableDictionary(kvp => kvp.Key, kvp => (TValue)kvp.Value);
        _semaphore.Release();

        return cache;
    }

    public async Task<TValue?> ReadByKeyAsync(string key)
    {
        await _semaphore.WaitAsync();

        if (CacheStorage.Cache.TryGetValue(key, out object? value))
            return await Task.FromResult((TValue)value);

        _semaphore.Release();

        return await Task.FromResult(default(TValue));
    }

    public async Task<IEnumerable<TValue>> ReadByValueAsync(Func<TValue, bool> predicate)
    {
        await _semaphore.WaitAsync();

        IEnumerable<TValue> values = CacheStorage.Cache.Where(kvp => kvp.Value is TValue)
                                                       .Select(kvp => (TValue)kvp.Value)
                                                       .Where(predicate);

        _semaphore.Release();

        return await Task.FromResult(values);
    }

    public async Task RemoveByAsync(string key)
    {
        await _semaphore.WaitAsync();

        if (CacheStorage.Cache.TryGetValue(key, out object? value))
        {
            CacheStorage.Cache.Remove(key);
            OnCacheChanged(new CacheChangedEventArgs<string, TValue>(CacheChangeType.Removed, key, (TValue)value));
        }

        _semaphore.Release();
    }

    public async Task WriteAsync(string key, TValue value)
    {
        await _semaphore.WaitAsync();

        CacheStorage.Cache[key] = value;

        OnCacheChanged(new CacheChangedEventArgs<string, TValue>(CacheChangeType.Added, key, value));

        _semaphore.Release();
    }

    private void OnCacheChanged(CacheChangedEventArgs<string, TValue> e)
        => Cache_Changed?.Invoke(this, e);
}