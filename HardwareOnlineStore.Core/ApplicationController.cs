using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Core.Initialization.Configuration.Abstractions;
using HardwareOnlineStore.Core.Initialization.IoC.Abstractions;
using HardwareOnlineStore.MVP.Presenters.Common;

namespace HardwareOnlineStore.Core;

public sealed class ApplicationController<TIoCContainer, TApplicationConfiguration> : IApplicationController
    where TIoCContainer : IIoCContainerBuilder, new()
    where TApplicationConfiguration : IApplicationConfigurationBuilder, new()
{
    public TIoCContainer Container { get; }

    public TApplicationConfiguration Configuration { get; }

    public ApplicationController()
        => (Container, Configuration) = (new TIoCContainer(), new TApplicationConfiguration());

    public void Run<TPresenter>()
        where TPresenter : class, IPresenter
    {
        TPresenter presenter = Container.Resolve<TPresenter>();
        presenter.Run();
    }

    public void Run<TPresenter>(Action action)
        where TPresenter : class, IPresenter
    {
        TPresenter presenter = Container.Resolve<TPresenter>();
        presenter.Run(action);
    }

    public TReturnedValue Run<TReturnedValue, TPresenter>()
        where TReturnedValue : notnull
        where TPresenter : class, IPresenter
    {
        TPresenter presenter = Container.Resolve<TPresenter>();
        TReturnedValue returnedValue = Container.Resolve<TReturnedValue>();

        presenter.Run();

        return returnedValue;
    }
}