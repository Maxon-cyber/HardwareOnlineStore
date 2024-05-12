using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using System.Collections.ObjectModel;

namespace HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;

public interface IProductShowcaseView : IView
{
    event Func<Task<ReadOnlyCollection<ProductModel>?>> LoadProducts;

    event Func<ProductModel, Task> AddProduct;
}