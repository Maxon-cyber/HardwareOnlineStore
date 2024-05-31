using HardwareOnlineStore.Services.Utilities.Caching.Abstractions;
using HardwareOnlineStore.Services.Utilities.Caching.Support.Serializer.Yaml;
using HardwareOnlineStore.Services.Utilities.Templates;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace HardwareOnlineStore.Services.Utilities.Caching.File;

public sealed class FileCache<TValue> : ICache<string, TValue>
    where TValue : notnull
{
    private FileInfoModel _fileInfo;
    private readonly YamlSerializer _serializer = new YamlSerializer();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 20);

    public event EventHandler<CacheChangedEventArgs<string, TValue>> CacheChanged;

    public DirectoryInfoModel Directory { get; }

    public string Separator { get; } = new string('-', 40);

    public bool IsEmpty => _fileInfo.Size == 0;

    public long SizeLimit => 8_192L;

    public FileCache(string path)
        => Directory = new DirectoryInfoModel(path);

    public sealed class FileEncryption
    {
        private static readonly byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private static readonly byte[] iv = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        private FileEncryption() { }

        public static void EncryptFile(string inputFile, string outputFile)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            using FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            using CryptoStream cs = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write);
            
            fsInput.CopyTo(cs);
        }

        public static void DecryptFile(string inputFile, string outputFile)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using FileStream fsEncrypted = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            using FileStream fsDecrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            using CryptoStream cs = new CryptoStream(fsEncrypted, decryptor, CryptoStreamMode.Read);
            
            cs.CopyTo(fsDecrypted);
        }
    }

    public FileCache<TValue> SetFile(string fileName)
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

    public async Task<IImmutableDictionary<string, TValue>> ReadAsync()
    {
        await _semaphore.WaitAsync();

        if (_fileInfo.Size == 0)
            return await Task.FromResult<IImmutableDictionary<string, TValue>>(ImmutableDictionary<string, TValue>.Empty);

        string[] content = await _fileInfo.ReadAsync();

        IImmutableDictionary<string, TValue> data = _serializer.Deserialize<Dictionary<string, TValue>>(content).ToImmutableDictionary();

        _semaphore.Release();

        return data;
    }

    public async Task<TValue?> ReadByKeyAsync(string key)
    {
        await _semaphore.WaitAsync();

        if (_fileInfo.Size == 0)
            return await Task.FromResult(default(TValue));

        string[] content = await _fileInfo.ReadByAsync(key, Separator);

        TValue? result = _serializer.Deserialize<TValue>(string.Join("\n", content));

        _semaphore.Release();

        return result;
    }

    public async Task WriteAsync(string key, TValue value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        await _semaphore.WaitAsync();

        if (_fileInfo.Size >= SizeLimit)
            while (_fileInfo.Size > SizeLimit / 2)
            {
                string[] lines = await _fileInfo.ReadAsync();

                await _fileInfo.WriteAsync(lines.Skip(Array.IndexOf(lines, Separator) + 1).ToArray(), WriteMode.WriteAll);
            }

        IDictionary<string, TValue> data = _fileInfo.Size != 0
            ? _serializer.Deserialize<IDictionary<string, TValue>>(await _fileInfo.ReadAsync())
            : new Dictionary<string, TValue>();

        data[key] = value;

        string serializedValues = _serializer.Serialize(data);
        
        await _fileInfo.WriteAsync(serializedValues, _fileInfo.Size != 0 ? WriteMode.WriteAll : WriteMode.Append);
        
        _semaphore.Release();
    }

    public async Task WriteWithEncryptionAsync(string key, TValue value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        await _semaphore.WaitAsync();

        if (_fileInfo.Size >= SizeLimit)
            while (_fileInfo.Size > SizeLimit / 2)
            {
                string[] lines = await _fileInfo.ReadAsync();

                await _fileInfo.WriteAsync(lines.Skip(Array.IndexOf(lines, Separator) + 1).ToArray(), WriteMode.WriteAll);
            }

        IDictionary<string, TValue> data = _fileInfo.Size != 0
            ? _serializer.Deserialize<IDictionary<string, TValue>>(await _fileInfo.ReadAsync())
            : new Dictionary<string, TValue>();

        data[key] = value;

        string serializedValues = _serializer.Serialize(data);

        await _fileInfo.WriteAsync(serializedValues, _fileInfo.Size != 0 ? WriteMode.WriteAll : WriteMode.Append);

        _semaphore.Release();
    }

    public async Task<bool> ContainsKeyAsync(string key)
    {
        await _semaphore.WaitAsync();

        if (_fileInfo.Size == 0)
            return false;

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

        //if (_fileInfo.Size == 0)
        //    return;

        //string[] content = await _fileInfo.ReadByAsync(key, Separator);
        //if (content.Length == 0)
        //    return;

        //string serializedValues = _serializer.Serialize(_data);
        //await _fileInfo.WriteAsync(serializedValues, WriteMode.WriteAll);

        //_semaphore.Release();
    }

    public async Task<string> SaveImageAsync(byte[] imageBytes, bool compress)
    {
        string path;
        do
        {
            path = Path.Combine(Directory.FullName, Path.GetRandomFileName() + ".tmp");
        } while (FileInfoModel.Exists(path));

        path = await Task.Run(() =>
        {
            using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);

            if (compress)
                image.Save(path, new JpegEncoder()
                {
                    Quality = 75
                });
            else
                image.Save(path);

            return path;
        });

        return path;
    }
}