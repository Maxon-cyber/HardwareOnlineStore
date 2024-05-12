using HardwareOnlineStore.MVP.Presenters.Common;

namespace HardwareOnlineStore.Core.Common.Abstractions;

public interface IApplicationController
{
    void Run<TPresenter>()
        where TPresenter : class, IPresenter;

    TReturnedValue Run<TReturnedValue, TPresenter>()
        where TReturnedValue : notnull
        where TPresenter : class, IPresenter;

    void Run<TPresenter>(Action action)
        where TPresenter : class, IPresenter;
}