using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Entities.Order;
using HardwareOnlineStore.Entities.Product;
using HardwareOnlineStore.Entities.User;
using HardwareOnlineStore.MVP.Presenters.Contracts;
using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.Services.Entity.SqlServerService;
using HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;
using HardwareOnlineStore.Services.Utilities.Caching.Abstractions;
using HardwareOnlineStore.Services.Utilities.Caching.Memory;
using System.Collections.ObjectModel;

namespace HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.ShoppingCart;

public sealed class ShoppingCartPresenter : Presenter<IShoppingCartView>
{
    private readonly MemoryCache<ProductModel> _cache;
    private readonly OrderService _service;

    public ShoppingCartPresenter(IApplicationController controller, IShoppingCartView view, SqlServerService service)
        : base(controller, view)
    {
        _service = service.Order;
        _cache = MemoryCache<ProductModel>.Instance;

        _cache.CacheChanged += Cache_Changed!;

        View.Order += Order;
        View.LoadOrders += LoadOrders;
    }

    private async Task<ReadOnlyCollection<OrderModel>?> LoadOrders()
    {
        IEnumerable<OrderEntity>? orders = await _service.SelectOrdersAsync();

        if (orders == null)
            return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(null);

        ReadOnlyCollection<OrderModel> ordersList = new ReadOnlyCollection<OrderModel>(
                 orders.Select(order => new OrderModel
                 {
                     UserId = order.UserId,
                     TotalAmount = order.TotalAmount,
                     Products = order.Products.Select(product => new ProductModel()
                     {
                         Name = product.Name,
                         Image = product.Image,
                         Price = product.Price,
                         Category = product.Category,
                         Quantity = product.Quantity
                     }).ToList(),
                     OrderDate = order.OrderDate,
                     Status = order.Status.ToString()
                 }).ToList());
       
        return ordersList;
    }

    private async Task Order(ICollection<ProductModel> products)
    {
        object? result = await _service.ChangeOrderAsync(Services.Entity.Contracts.Abstractions.TypeOfUpdateCommand.Insert, new OrderEntity()
        {
            UserId = _cache.Of<UserEntity>().FirstOrDefault().Id,
            Products = products.Select(product => new ProductEntity()
            {
                Name = product.Name,
                Image = product.Image,
                Price = product.Price,
                Category = product.Category,
                Quantity = product.Quantity
            }).ToList(),
            TotalAmount = products.Sum(p => p.Price),
            OrderDate = DateTime.Now,
            Status = Status.InProcessing
        });
    }

    private void Cache_Changed(object sender, CacheChangedEventArgs<string, ProductModel> cacheChanged)
    {
        switch (cacheChanged.ChangeType)
        {
            case CacheChangeType.Added:
                View.AddProduct(cacheChanged.Value);
                break;
            case CacheChangeType.Updated:
                break;
            case CacheChangeType.Removed:
                break;
        }
    }
}