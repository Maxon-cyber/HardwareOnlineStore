namespace HardwareOnlineStore.MVP.Views.Abstractions.Shared;

public enum MessageLevel
{
    Info = 0,
    Warning = 1,
    Error = 2,
}

public interface IView
{
    void Show();

    void ShowMessage(string message, string caption, MessageLevel level);

    void Close();
}