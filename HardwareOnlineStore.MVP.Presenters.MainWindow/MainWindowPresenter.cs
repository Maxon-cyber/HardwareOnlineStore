using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.MVP.Presenters.Contracts;
using HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.ProductShowcase;
using HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.ShoppingCart;
using HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.UserAccount;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;

namespace HardwareOnlineStore.MVP.Presenters.MainWindow;

public sealed class MainWindowPresenter : Presenter<IMainWindowView>
{
    public MainWindowPresenter(IApplicationController controller, IMainWindowView view)
        : base(controller, view)
    {
        View.OpenUserAccount += OpenUserAccount;
        View.OpenProductShowcase += OpenProductShowcase;
        View.OpenShoppingCart += OpenShoppingCart;
    }

    private IUserAccountView OpenUserAccount()
       => Controller.Run<IUserAccountView, UserAccountPresenter>();

    private IProductShowcaseView OpenProductShowcase()
       => Controller.Run<IProductShowcaseView, ProductShowcasePresenter>();

    private IShoppingCartView OpenShoppingCart()
       => Controller.Run<IShoppingCartView, ShoppingCartPresenter>();
}