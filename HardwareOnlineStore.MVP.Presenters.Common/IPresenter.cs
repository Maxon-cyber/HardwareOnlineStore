namespace HardwareOnlineStore.MVP.Presenters.Common;

public interface IPresenter
{
    void Run();

    void Run(Action action);
}