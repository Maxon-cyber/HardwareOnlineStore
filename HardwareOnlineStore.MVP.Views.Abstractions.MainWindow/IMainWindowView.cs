using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;

namespace HardwareOnlineStore.MVP.Views.Abstractions.MainWindow;

public interface IMainWindowView : IView
{
    event Func<IUserAccountView> OpenUserAccount;
    event Func<IProductShowcaseView> OpenProductShowcase;
    event Func<IShoppingCartView> OpenShoppingCart;
}