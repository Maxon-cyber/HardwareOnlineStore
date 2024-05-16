using HardwareOnlineStore.Core;
using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Core.Initialization.Configuration.Microsoft;
using HardwareOnlineStore.Core.Initialization.IoC.Abstractions;
using HardwareOnlineStore.Core.Initialization.IoC.Autofac;
using HardwareOnlineStore.DataAccess.Providers.Relational.Abstractions;
using HardwareOnlineStore.DataAccess.Repositories.Relational.SqlServer;
using HardwareOnlineStore.MVP.Presenters.MainWindow;
using HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.ProductShowcase;
using HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.ShoppingCart;
using HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.UserAccount;
using HardwareOnlineStore.MVP.Presenters.UserIdentification;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.MVP.Views.Abstractions.UserIdentification;
using HardwareOnlineStore.MVP.Views.MainWindow;
using HardwareOnlineStore.MVP.Views.MainWindow.Sections;
using HardwareOnlineStore.MVP.Views.UserIdentification.Authorization;
using HardwareOnlineStore.MVP.Views.UserIdentification.Registration;
using HardwareOnlineStore.Services.Entity.SqlServerService;
using HardwareOnlineStore.Services.Utilities.Caching.File;
using HardwareOnlineStore.Services.Utilities.Logger.File;
using Microsoft.Extensions.Configuration;

namespace HardwareOnlineStore.MVP.Views.UserIdentification;

internal static class Program
{
    private static readonly ApplicationContext _context = new ApplicationContext();

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        Application.SetCompatibleTextRenderingDefault(false);

        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        ApplicationController<AutofacBuilder, MicrosoftConfigurationBuilder> applicationController = new ApplicationController<AutofacBuilder, MicrosoftConfigurationBuilder>();

        applicationController.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                                           .AddFile("config.yml", true, false)
                                           .AddEnviromentVariables()
                                           .Build();

        IConfigurationSection sqlServerSection = applicationController.Configuration.Root.GetSection("Databases:SqlServer");
        IDictionary<string, ConnectionParameters> parametersOfAllDatabases = new Dictionary<string, ConnectionParameters>
        {
            {
                sqlServerSection.Key,
                new ConnectionParameters()
                {
                    Provider = sqlServerSection["Key"]!,
                    Server = sqlServerSection["Server"]!,
                    Port = Convert.ToInt32(sqlServerSection["Port"]),
                    Database = sqlServerSection["Database"]!,
                    IntegratedSecurity = Convert.ToBoolean(sqlServerSection["IntegratedSecurity"]),
                    Username = sqlServerSection["Username"],
                    Password = sqlServerSection["Password"],
                    TrustedConnection = Convert.ToBoolean(sqlServerSection["TrustedConnection"]),
                    TrustServerCertificate = Convert.ToBoolean(sqlServerSection["TrustServerCertificate"]),
                    ConnectionTimeout = TimeSpan.Parse(sqlServerSection["ConnectionTimeout"]!),
                    MaxPoolSize = Convert.ToInt32(sqlServerSection["MaxPoolSize"])
                }
            },
        };

        applicationController.Container.Register<ShoppingCartPresenter>(asSelf: true)
                                       .Register<ProductShowcasePresenter>(asSelf: true)
                                       .Register<UserAccountPresenter>(asSelf: true)
                                       .Register<MainWindowPresenter>(asSelf: true)
                                       .Register<RegistrationPresenter>(asSelf: true)
                                       .Register<AuthorizationPresenter>(asSelf: true)
                                       .RegisterView<IMainWindowView, MainWindowForm>(Lifetime.Singleton)
                                       .RegisterView<IUserAccountView, UserAccountControl>(Lifetime.Singleton)
                                       .RegisterView<IProductShowcaseView, ProductShowcaseControl>(Lifetime.Singleton)
                                       .RegisterView<IShoppingCartView, ShoppingCartControl>(Lifetime.Singleton)
                                       .RegisterView<IRegistrationView, RegistrationControl>(Lifetime.Singleton)
                                       .RegisterView<IAuthorizationView, AuthorizationForm>(Lifetime.Singleton)
                                       .RegisterWithConstructor<SqlServerRepository>("connectionParameters", parametersOfAllDatabases["SqlServer"])
                                       .RegisterWithConstructor<FileLogger>("path", applicationController.Configuration.Root.GetSection("Logging:Path").Value!, Lifetime.Singleton)
                                       .RegisterGenericWithConstructor(typeof(CachedFileManager<>), "path", applicationController.Configuration.Root.GetSection("Caching:Path").Value!, Lifetime.Singleton)
                                       .Register<SqlServerService>(Lifetime.Singleton)
                                       .RegisterInstance(_context, Lifetime.Singleton)
                                       .RegisterInstance<IApplicationController>(applicationController)
                                       .Build();

        applicationController.Run<AuthorizationPresenter>();
    }
}