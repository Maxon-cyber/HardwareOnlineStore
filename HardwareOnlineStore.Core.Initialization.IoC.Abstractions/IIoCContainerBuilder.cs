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
        where TService : notnull
        where TImplementation : notnull, TService;

    IIoCContainerBuilder RegisterGeneric(Type type, Lifetime lifetime = Lifetime.Transient);

    IIoCContainerBuilder RegisterGenericWithConstructor(Type type, string nameParameter, object parameter, Lifetime lifetime = Lifetime.Transient, bool aasSelf = false);

    IIoCContainerBuilder RegisterWithConstructor<TService>(string nameParameter, object parameter, Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TService : notnull;

    TService Resolve<TService>()
       where TService : notnull;

    void Build();
}