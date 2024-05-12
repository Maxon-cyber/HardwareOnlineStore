﻿using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Entities.Product;
using HardwareOnlineStore.MVP.Presenters.Contracts;
using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using HardwareOnlineStore.Services.Entity.SqlServerService;
using HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;
using HardwareOnlineStore.Services.Utilities.Caching.Memory;
using System.Collections.ObjectModel;

namespace HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.ProductShowcase;

public sealed class ProductShowcasePresenter : Presenter<IProductShowcaseView>
{
    private readonly ProductService _service;
    private readonly MemoryCache<ProductModel> _cache;

    public ProductShowcasePresenter(IApplicationController controller, IProductShowcaseView view, SqlServerService service)
        : base(controller, view)
    {
        _service = service.Product;
        _cache = MemoryCache<ProductModel>.Instance;

        View.LoadProducts += LoadProductsAsync;
        View.AddProduct += AddProduct;
    }

    private async Task<ReadOnlyCollection<ProductModel>?> LoadProductsAsync()
    {
        IEnumerable<ProductEntity>? products = await _service.SelectProductsAsync();

        if (products == null)
        {
            View.ShowMessage("Не удалось загрузить продукты", "Ошибка", MessageLevel.Error);
            return await Task.FromResult<ReadOnlyCollection<ProductModel>?>(null);
        }

        ProductEntity[] arrayProducts = products.ToArray();
        List<ProductModel> productModels = [];

        for (int index = 0; index < arrayProducts.Length; index++)
        {
            ProductEntity currentProduct = arrayProducts[index];
            productModels.Add(new ProductModel()
            {
                Name = currentProduct.Name,
                Image = currentProduct.Image,
                Quantity = currentProduct.Quantity,
                Category = currentProduct.Category,
                Price = currentProduct.Price
            });
        }

        ReadOnlyCollection<ProductModel> readOnlyProducts = new ReadOnlyCollection<ProductModel>(productModels);

        return readOnlyProducts;
    }

    private async Task AddProduct(ProductModel product)
        => await _cache.WriteAsync(product.Name, product);
}