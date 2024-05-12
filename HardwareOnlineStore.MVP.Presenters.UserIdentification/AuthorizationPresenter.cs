using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Entities.User;
using HardwareOnlineStore.MVP.Presenters.Contracts;
using HardwareOnlineStore.MVP.Presenters.MainWindow;
using HardwareOnlineStore.MVP.ViewModels.UserIdentification;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using HardwareOnlineStore.MVP.Views.Abstractions.UserIdentification;
using HardwareOnlineStore.Services.Entity.SqlServerService;
using HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;
using HardwareOnlineStore.Services.Utilities.Caching.Memory;
using System.Text;

namespace HardwareOnlineStore.MVP.Presenters.UserIdentification;

public sealed class AuthorizationPresenter : Presenter<IAuthorizationView>
{
    private readonly UserService _userService;
    private readonly MemoryCache<UserEntity> _memoryCache;

    public AuthorizationPresenter(IApplicationController controller, IAuthorizationView view, SqlServerService service)
        : base(controller, view)
    {
        _userService = service.User;
        _memoryCache = MemoryCache<UserEntity>.Instance;

        View.Authorization += LoginAsync;
        View.Registration += Registration;
    }

    private async Task LoginAsync(AuthorizationViewModel model)
    {
        UserEntity? user = await _userService.GetUserAsync(new UserEntity
        {
            Login = model.Login,
            Password = Encoding.UTF8.GetBytes(model.Password)
        });

        if (user == null)
        {
            View.ShowMessage("Неправильный логин или пароль", "Ошибка", MessageLevel.Error);
            return;
        }

        View.ShowMessage($"Добро Пожаловать, {user}!", "Добро пожаловать", MessageLevel.Info);

        switch (user.Role)
        {
            case Role.User:
                Controller.Run<MainWindowPresenter>();
                await _memoryCache.WriteAsync("User", user);
                View.Close();
                break;
            case Role.Admin:
                await _memoryCache.WriteAsync("Admin", user);
                View.Close();
                break;
        }
    }

    private IRegistrationView Registration()
        => Controller.Run<IRegistrationView, RegistrationPresenter>();
}