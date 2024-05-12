using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using System.Collections.ObjectModel;

namespace HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;

public interface IShoppingCartView : IView
{
    event Func<ICollection<ProductModel>, Task> Order;
    event Func<Task<ReadOnlyCollection<OrderModel>?>> LoadOrders;

    void AddProduct(ProductModel product);
}