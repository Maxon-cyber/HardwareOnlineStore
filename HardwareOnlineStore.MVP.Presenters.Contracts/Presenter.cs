using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.MVP.Presenters.Common;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;

namespace HardwareOnlineStore.MVP.Presenters.Contracts;

public abstract class Presenter<TView> : IPresenter
    where TView : IView
{
    protected TView View { get; }

    protected IApplicationController Controller { get; }

    protected Presenter(IApplicationController controller, TView view)
        => (Controller, View) = (controller, view);

    public void Run()
        => View.Show();

    public void Run(Action action)
    {
        action();
        View.Show();
    }
}