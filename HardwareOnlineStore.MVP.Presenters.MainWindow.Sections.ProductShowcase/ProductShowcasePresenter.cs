using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Entities.Product;
using HardwareOnlineStore.Entities.User;
using HardwareOnlineStore.MVP.Presenters.Contracts;
using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using HardwareOnlineStore.Services.Entity.SqlServerService;
using HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;
using HardwareOnlineStore.Services.Utilities.Caching.Memory;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.ProductShowcase;

public sealed class ProductShowcasePresenter : Presenter<IProductShowcaseView>
{
    private readonly ProductService _service;
    private readonly MemoryCache<ProductModel> _memoryCache;

    public ProductShowcasePresenter(IApplicationController controller, IProductShowcaseView view, SqlServerService service)
        : base(controller, view)
    {
        _service = service.Product;
        _memoryCache = MemoryCache<ProductModel>.Instance;

        View.LoadProducts += LoadProductsAsync;
        View.AddProduct += AddProduct;
    }

    private async Task<ReadOnlyCollection<ProductModel>?> LoadProductsAsync()
    {
        if (!UserParameters.Internet.IsAvailable())
        {
            View.ShowMessage("Не удалось загрузить продукты\nПроверьте подключение к интернету", "Ой...", MessageLevel.Error);
            return await Task.FromResult<ReadOnlyCollection<ProductModel>?>(null);
        }

        IEnumerable<ProductEntity>? products = await _service.SelectProductsAsync();

        if (products == null)
        {
            View.ShowMessage("Не удалось загрузить продукты\nПопробуйте позже", "Ой...", MessageLevel.Error);
            return await Task.FromResult<ReadOnlyCollection<ProductModel>?>(null);
        }

        ConcurrentBag<ProductModel> productModels = [];
        ProductEntity[] arrayProducts = [.. products];

        for (int productIndex = 0; productIndex < arrayProducts.Length; productIndex++)
        {
            ProductEntity currentProduct = arrayProducts[productIndex];
            productModels.Add(new ProductModel()
            {
                Id = currentProduct.Id,
                Name = currentProduct.Name,
                Image = currentProduct.Image,
                Quantity = currentProduct.Quantity,
                Category = currentProduct.Category,
                Price = currentProduct.Price
            });
        }

        ReadOnlyCollection<ProductModel> readOnlyProducts = new ReadOnlyCollection<ProductModel>([.. productModels]);

        return readOnlyProducts;
    }

    private async Task AddProduct(ProductModel product)
        => await _memoryCache.WriteAsync(product.Name, product);
}