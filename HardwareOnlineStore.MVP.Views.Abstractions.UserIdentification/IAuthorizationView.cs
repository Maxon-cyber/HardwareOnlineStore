using HardwareOnlineStore.MVP.ViewModels.UserIdentification;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;

namespace HardwareOnlineStore.MVP.Views.Abstractions.UserIdentification;

public interface IAuthorizationView : IView
{
    event Func<AuthorizationViewModel, bool, Task> Authorization;
    event Func<IRegistrationView> Registration;
}