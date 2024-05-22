using HardwareOnlineStore.Services.Utilities.Logger.Abstractions;
using HardwareOnlineStore.Services.Utilities.Templates;
using System.Collections;

namespace HardwareOnlineStore.Services.Utilities.Logger.File;

public sealed class FileLogger : ILogger
{
    private FileInfoModel _fileInfo;

    public DirectoryInfoModel Directory { get; }

    public string MessagePattern { get; } = "{0:yyyy-MM-dd HH:mm:ss} [{1}]: {2}";

    public string Separator { get; } = new string('=', 100);

    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 20);

    public FileLogger(string path)
        => Directory = new DirectoryInfoModel(path);

    public FileLogger SetFile(string fileName)
    {
        _fileInfo = Directory[fileName]
            ?? throw new FileNotFoundException($"Файл {Path.Combine(Directory.FullName, fileName)} не найден");

        return this;
    }

    public async Task LogInfoAsync(string message)
    {
        await _semaphore.WaitAsync();

        if (_fileInfo.Size >= _fileInfo.SizeLimit)
            while (_fileInfo.Size > _fileInfo.SizeLimit / 2)
            {
                string[] lines = await _fileInfo.ReadAsync();

                await _fileInfo.WriteAsync(lines.Skip(Array.IndexOf(lines, Separator) + 1).ToArray(), WriteMode.WriteAll);
            }

        await _fileInfo.WriteAsync(string.Format(MessagePattern, DateTime.Now, "Info", message), WriteMode.Append);

        _semaphore.Release();
    }

    public async Task LogErrorAsync(Exception exception, string message)
    {
        await _semaphore.WaitAsync();

        if (_fileInfo.Size >= _fileInfo.SizeLimit)
            while (_fileInfo.Size > _fileInfo.SizeLimit / 2)
            {
                string[] lines = await _fileInfo.ReadAsync();

                await _fileInfo.WriteAsync(lines.Skip(Array.IndexOf(lines, Separator) + 1).ToArray(), WriteMode.WriteAll);
            }

        await _fileInfo.WriteAsync(string.Format(MessagePattern, DateTime.Now, "Error", message), WriteMode.Append);

        await _fileInfo.WriteAsync($"Тип ошибки: {exception.GetType()}", WriteMode.Append);

        await _fileInfo.WriteAsync("Доп. данные об ошибке:", WriteMode.Append);

        string additionalData = null!;
        foreach (DictionaryEntry data in exception.Data)
            additionalData += $"\t{data.Key} - {data.Value}";

        await _fileInfo.WriteAsync(additionalData, WriteMode.Append);

        _semaphore.Release();
    }

    public async Task LogSeparatorAsync()
    {
        await _semaphore.WaitAsync();

        await _fileInfo.WriteAsync(Separator, WriteMode.Append);

        _semaphore.Release();
    }

    public async Task LogMessageAsync(string message)
    {
        await _semaphore.WaitAsync();

        if (_fileInfo.Size >= _fileInfo.SizeLimit)
            while (_fileInfo.Size > _fileInfo.SizeLimit / 2)
            {
                string[] lines = await _fileInfo.ReadAsync();

                await _fileInfo.WriteAsync(lines.Skip(Array.IndexOf(lines, Separator) + 1).ToArray(), WriteMode.WriteAll);
            }

        await _fileInfo.WriteAsync(message, WriteMode.Append);

        _semaphore.Release();
    }
}