using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Entities.Order;
using HardwareOnlineStore.Entities.Product;
using HardwareOnlineStore.Entities.User;
using HardwareOnlineStore.MVP.Presenters.Contracts;
using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.Services.Entity.Contracts;
using HardwareOnlineStore.Services.Entity.SqlServerService;
using HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;
using HardwareOnlineStore.Services.Utilities.Caching.Abstractions;
using HardwareOnlineStore.Services.Utilities.Caching.File;
using HardwareOnlineStore.Services.Utilities.Caching.Memory;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.ShoppingCart;

public sealed class ShoppingCartPresenter : Presenter<IShoppingCartView>
{
    private readonly MemoryCache<ProductModel> _cache;
    private readonly CachedFileManager<OrderModel> _cachedFile;
    private readonly OrderService _orderService;
    private readonly ProductService _productService;

    public ShoppingCartPresenter(IApplicationController controller, IShoppingCartView view, SqlServerService service, CachedFileManager<OrderModel> cachedFile)
        : base(controller, view)
    {
        _orderService = service.Order;
        _productService = service.Product;

        _cache = MemoryCache<ProductModel>.Instance;
        _cachedFile = cachedFile.SetFile("Order");

        _cache.CacheChanged += Cache_Changed!;

        View.Order += Order;
        View.LoadOrders += LoadOrders;
    }

    private async Task<ReadOnlyCollection<OrderModel>?> LoadOrders()
    {
        if (!UserParameters.Internet.IsAvailable())
        {
            IImmutableDictionary<string, OrderModel>? orderModels = await _cachedFile.ReadAsync();

            if (orderModels == null)
                return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(null);

            return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(orderModels.Values.ToList().AsReadOnly());
        }

        IEnumerable<OrderEntity>? orders = await _orderService.SelectOrdersAsync();

        if (orders == null)
            return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(null);

        List<OrderModel> orderList = [];

        foreach (OrderEntity order in orders)
        {
            List<Guid> ids = order.Items.Select(o => o.ProductId).ToList();
            IEnumerable<ProductEntity>? products = await _productService.GetProductsByIdsAsync(ids);

            if (products == null)
            {
                View.ShowMessage("Не удалось загрузить данные о продуктов", "Ошибка", Views.Abstractions.Shared.MessageLevel.Error);
                return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(null);
            }

            List<ProductModel> productModels = [];

            foreach (ProductEntity product in products)
                productModels.Add(new ProductModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Image = product.Image,
                    Price = product.Price,
                    Category = product.Category,
                    Quantity = product.Quantity
                });

            orderList.Add(new OrderModel
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Products = productModels,
                OrderDate = order.TimeCreated,
                Status = order.Status.ToString()
            });
        }

        return orderList.AsReadOnly();
    }

    private async Task Order(ICollection<ProductModel> products)
    {
        var groupedProducts = products.GroupBy(p => p.Id)
                                      .Select(g => new
                                      {
                                          ProductId = g.Key,
                                          NumberOfProducts = g.Count()
                                      }).ToList();

        OrderEntity orderEntity = new OrderEntity()
        {
            UserId = _cache.Of<UserEntity>()!.FirstOrDefault()!.Id,
            Items = groupedProducts.Select(gp => new OrderItem()
            {
                ProductId = gp.ProductId,
                NumberOfProducts = gp.NumberOfProducts
            }).ToList(),
            TotalAmount = products.Sum(p => p.Price),
            DeliveryDate = DateTime.Now.AddDays(5),
            Status = Status.InProcessing
        };

        if (UserParameters.Internet.IsAvailable())
        {
            object? result = await _orderService.ChangeOrderAsync(TypeOfUpdateCommand.Insert, orderEntity);

            View.ShowMessage("Заказ успешно оформлен", "Успех", Views.Abstractions.Shared.MessageLevel.Info);
        }

        OrderModel orderModel = new OrderModel()
        {
            UserId = orderEntity.UserId,
            Products = [],
            TotalAmount = orderEntity.TotalAmount,
            OrderDate = orderEntity.TimeCreated,
            Status = orderEntity.Status.ToString()
        };

        foreach (ProductModel product in products)
            orderModel.Products.Add(new ProductModel()
            {
                Id = product.Id,
                Name = product.Name,
                Image = _cachedFile.SaveImage((product.Image as byte[])!, product.Name, true),
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity
            });

        await _cachedFile.WriteAsync($"{orderModel.UserId}. Order Date: {orderModel.OrderDate}", orderModel);
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