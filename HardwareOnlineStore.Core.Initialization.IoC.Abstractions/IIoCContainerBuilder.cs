namespace HardwareOnlineStore.Core.Initialization.IoC.Abstractions;

public enum Lifetime
{
    Transient = 0,
    Singleton = 1
}

public interface IIoCContainerBuilder
{
    IIoCContainerBuilder Register<TService>(Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TService : notnull;

    IIoCContainerBuilder RegisterInstance<TInstance>(TInstance instance, Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TInstance : class;

    IIoCContainerBuilder RegisterView<TView, TImplementation>(Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TView : notnull
        where TImplementation : notnull, TView;

    IIoCContainerBuilder Register<TService, TImplementation>(Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TImplementation : notnull, TService;

    TService Resolve<TService>()
        where TService : notnull;

    IIoCContainerBuilder RegisterGeneric<TService>(Lifetime lifetime)
        where TService : notnull;

    IIoCContainerBuilder RegisterWithConstructor<TService>(string nameParameter, object parameter, Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TService : notnull;

    IIoCContainerBuilder RegisterGeneric<TService, TImplementation>(Lifetime lifetime)
        where TImplementation : notnull, TService;

    void Build();
}