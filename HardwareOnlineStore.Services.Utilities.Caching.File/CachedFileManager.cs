using HardwareOnlineStore.Services.Utilities.Caching.Abstractions;
using HardwareOnlineStore.Services.Utilities.Caching.Support.Serializer.Yaml;
using HardwareOnlineStore.Services.Utilities.Templates;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Immutable;

namespace HardwareOnlineStore.Services.Utilities.Caching.File;

public sealed class CachedFileManager<TValue> : ICache<string, TValue>
{
    private FileInfoModel _fileInfo;
    private readonly YamlSerializer _serializer = new YamlSerializer();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 20);

    public event EventHandler<CacheChangedEventArgs<string, TValue>> CacheChanged;

    public DirectoryInfoModel Directory { get; }

    public string Separator { get; } = new string('-', 40);

    public bool IsEmpty => _fileInfo.Size == 0;

    public CachedFileManager(string path)
        => Directory = new DirectoryInfoModel(path);

    public CachedFileManager<TValue> SetFile(string fileName)
    {
        _fileInfo = Directory[fileName]
            ?? throw new FileNotFoundException($"Файл {Path.Combine(Directory.FullName, fileName)} не найден");

        return this;
    }

    public IEnumerable<FileInfoModel> Of(string extension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);

        IReadOnlyCollection<FileInfoModel> files = Directory.GetFiles();

        return files.Where(f => f.Extension == extension);
    }

    public async Task<IImmutableDictionary<string, TValue>?> ReadAsync()
    {
        await _semaphore.WaitAsync();

        string[] content = await _fileInfo.ReadAsync();

        if (content.Length == 0)
            return null;

        ImmutableDictionary<string, TValue> data = _serializer.Deserialize<Dictionary<string, TValue>>(content).ToImmutableDictionary();

        _semaphore.Release();

        return data;
    }

    public async Task<TValue?> ReadByKeyAsync(string key)
    {
        await _semaphore.WaitAsync();

        string[] content = await _fileInfo.ReadByAsync(key, Separator);

        if (content.Length == 0)
            return await Task.FromResult(default(TValue));

        TValue? result = _serializer.Deserialize<TValue>(string.Join("\n", content));

        _semaphore.Release();

        return result;
    }

    public async Task WriteAsync(string key, TValue value)
    {
        await _semaphore.WaitAsync();

        string serializedValues = _serializer.Serialize(new Dictionary<string, TValue>()
        {
            {
                key,
                value
            },
        });

        await _fileInfo.WriteAsync(serializedValues, WriteMode.Append);

        _semaphore.Release();
    }

    public async Task<bool> ContainsKeyAsync(string key)
    {
        await _semaphore.WaitAsync();

        string[] content = await _fileInfo.ReadByAsync(key, Separator);

        bool isContains = content.Length == 0;

        _semaphore.Release();

        return isContains;
    }

    public async Task ClearAsync()
    {
        await _semaphore.WaitAsync();

        await _fileInfo.ClearAsync();

        _semaphore.Release();
    }

    public async Task RemoveByAsync(string key)
    {
        //await _semaphore.WaitAsync();

        //if (_data == null)
        //    return;

        //_data = _data.Where(dict => !dict.Key.Equals(key)).ToImmutableDictionary();

        //string serializedValues = _serializer.Serialize(_data);
        //await _fileInfo.WriteAsync(serializedValues, WriteMode.WriteAll);

        //_semaphore.Release();
    }

    public string SaveImage(byte[] imageBytes, string imageName, bool compress)
    {
        using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);

        string path = Directory.FullName + @$"\{imageName}.jpeg";

        if (compress)
        {
            JpegEncoder encoder = new JpegEncoder()
            {
                Quality = 75
            };

            image.Save(path, encoder);
        }
        else
            image.Save(path);

        return path;
    }
}