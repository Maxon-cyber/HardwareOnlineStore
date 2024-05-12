namespace HardwareOnlineStore.Services.Utilities.Templates;

public sealed class DirectoryInfoModel
{
    private readonly DirectoryInfo _directoryInfo;
    private readonly List<FileInfoModel> _files;

    public string Name => _directoryInfo.Name;

    public string FullName => _directoryInfo.FullName;

    public DateTime LastAccessTime => _directoryInfo.LastAccessTime;

    public DateTime LastWriteTime => _directoryInfo.LastWriteTime;

    public DirectoryInfoModel(string path)
    {
        _directoryInfo = new DirectoryInfo(path);

        FileInfo[] files = _directoryInfo.GetFiles();
        _files = [];

        foreach (FileInfo file in files)
            _files.Add(new FileInfoModel(file.FullName));
    }

    public FileInfoModel? this[string shortFileName]
    {
        get
        {
            foreach (FileInfoModel file in _files)
                if (string.Equals(file.Name, shortFileName, StringComparison.CurrentCultureIgnoreCase))
                    return file;

            return null;
        }
    }
}