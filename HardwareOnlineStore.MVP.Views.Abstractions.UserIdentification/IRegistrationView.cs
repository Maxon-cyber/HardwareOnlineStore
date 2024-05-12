using HardwareOnlineStore.MVP.ViewModels.UserIdentification;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;

namespace HardwareOnlineStore.MVP.Views.Abstractions.UserIdentification;

public interface IRegistrationView : IView
{
    event Func<RegistrationViewModel, Task> Registration;
    event Action? ReturnToAuthorization;
}