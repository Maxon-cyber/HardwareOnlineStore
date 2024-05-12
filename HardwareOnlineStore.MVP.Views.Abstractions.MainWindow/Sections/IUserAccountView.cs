using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;

namespace HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;

public interface IUserAccountView : IView
{
    event Func<Task<UserAccountModel?>> LoadUserData;
    event Func<UserAccountModel, Task> UpdateData;
}