using Autofac;
using HardwareOnlineStore.Core.Initialization.IoC.Abstractions;

namespace HardwareOnlineStore.Core.Initialization.IoC.Autofac;

public sealed class AutofacBuilder() : IIoCContainerBuilder
{
    private static bool _isBuild = false;
    private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();
    private IContainer _container;

    public IIoCContainerBuilder Register<TService>(Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TService : notnull
    {
        switch (lifetime)
        {
            case Lifetime.Transient:
                _containerBuilder.RegisterType<TService>().Named<TService>($"{nameof(TService)}")
                    .AsSelf();
                break;
            case Lifetime.Singleton:
                _containerBuilder.RegisterType<TService>().Named<TService>($"{nameof(TService)}")
                    .AsSelf()
                    .SingleInstance();
                break;
        }

        return this;
    }

    public IIoCContainerBuilder RegisterWithConstructor<TService>(string nameParameter, object parameter, Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TService : notnull
    {
        switch (lifetime)
        {
            case Lifetime.Transient:
                _containerBuilder.RegisterType<TService>().Named<TService>($"{nameof(TService)}")
                    .AsSelf()
                    .WithParameter(nameParameter, parameter);
                break;
            case Lifetime.Singleton:
                _containerBuilder.RegisterType<TService>().Named<TService>($"{nameof(TService)}")
                    .AsSelf()
                    .SingleInstance()
                    .WithParameter(nameParameter, parameter);
                break;
        }

        return this;
    }

    public IIoCContainerBuilder RegisterInstance<TInstance>(TInstance instance, Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TInstance : class
    {
        switch (lifetime)
        {
            case Lifetime.Transient:
                _containerBuilder.RegisterInstance(instance).Named<TInstance>($"{nameof(TInstance)}")
                    .As<TInstance>();
                break;
            case Lifetime.Singleton:
                _containerBuilder.RegisterInstance(instance).Named<TInstance>($"{nameof(TInstance)}")
                    .As<TInstance>()
                    .SingleInstance();
                break;
        }

        return this;
    }

    public IIoCContainerBuilder RegisterView<TView, TImplementation>(Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TView : notnull
        where TImplementation : notnull, TView
    {
        switch (lifetime)
        {
            case Lifetime.Transient:
                _containerBuilder.RegisterType<TImplementation>().Named<TImplementation>($"{nameof(TView)} - {nameof(TImplementation)}")
                    .As<TView>()
                    .AsSelf()
                    .InstancePerDependency();
                break;
            case Lifetime.Singleton:
                _containerBuilder.RegisterType<TImplementation>().Named<TImplementation>($"{nameof(TView)} - {nameof(TImplementation)}")
                    .As<TView>()
                    .AsSelf()
                    .SingleInstance();
                break;
        }

        return this;
    }

    public IIoCContainerBuilder RegisterGeneric(Type type, Lifetime lifetime = Lifetime.Transient)
    {
        ArgumentNullException.ThrowIfNull(type);

        switch (lifetime)
        {
            case Lifetime.Transient:
                _containerBuilder.RegisterGeneric(type).Named($"{nameof(type)}", type)
                    .AsSelf()
                    .InstancePerDependency();
                break;
            case Lifetime.Singleton:
                _containerBuilder.RegisterGeneric(type).Named($"{nameof(type)}", type)
                    .AsSelf()
                    .SingleInstance();
                break;
        }

        return this;
    }

    public IIoCContainerBuilder RegisterGenericWithConstructor(Type type, string nameParameter, object parameter, Lifetime lifetime = Lifetime.Transient, bool aasSelf = false)
    {
        switch (lifetime)
        {
            case Lifetime.Transient:
                _containerBuilder.RegisterGeneric(type).Named(type.Name, type)
                    .AsSelf()
                    .WithParameter(nameParameter, parameter);
                break;
            case Lifetime.Singleton:
                _containerBuilder.RegisterGeneric(type).Named(type.Name, type)
                    .AsSelf()
                    .SingleInstance()
                    .WithParameter(nameParameter, parameter);
                break;
        }

        return this;
    }

    public IIoCContainerBuilder Register<TService, TImplementation>(Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        switch (lifetime)
        {
            case Lifetime.Transient:
                _containerBuilder.RegisterType<TImplementation>().Named<TImplementation>($"{nameof(TService)} - {nameof(TImplementation)}")
                    .As<TService>()
                    .AsSelf()
                    .InstancePerDependency();
                break;
            case Lifetime.Singleton:
                _containerBuilder.RegisterType<TImplementation>().Named<TImplementation>($"{nameof(TService)} - {nameof(TImplementation)}")
                    .As<TService>()
                    .AsSelf()
                    .SingleInstance();
                break;
        }

        return this;
    }

    public TService Resolve<TService>()
        where TService : notnull
    {
        if (!_isBuild)
            _container = _containerBuilder.Build();

        using ILifetimeScope lifetimeScope = _container.BeginLifetimeScope();

        if (!lifetimeScope.IsRegistered<TService>())
            throw new ArgumentException($"{typeof(TService)} не зарегистрирован");

        TService service = lifetimeScope.Resolve<TService>();

        return service;
    }

    public void Build()
    {
        if (_isBuild)
            return;

        _container = _containerBuilder.Build();
        _isBuild = true;
    }
}