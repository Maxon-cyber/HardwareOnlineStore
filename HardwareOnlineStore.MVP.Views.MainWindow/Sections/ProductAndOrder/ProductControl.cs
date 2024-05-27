using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.MainWindow.Sections.ProductAndOrder.Common;

namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections.ProductAndOrder;

public sealed partial class ProductControl : UserControl
{
    private readonly ProductModel _product;

    public event EventHandler? AddButtonClicked;
    public event EventHandler? DeleteButtonClicked;

    public ProductControl(ProductModel product)
    {
        InitializeComponent();
        _product = product;
    }

    public async Task CreateProductViewAsync()
    {
        if (AddButtonClicked == null && DeleteButtonClicked == null)
            throw new ArgumentNullException("AddButtonClicked/DeleteButtonClicked", "Перед созданием представления продукта необходимо подписаться хотя бы на одно свойство: AddButtonClicked или DeleteButtonClicked");

        productPictureBox.Name = $"{_product.Name}PictureBox";

        if (!Path.IsPathRooted(_product.Image.ToString()))
        {
            await using MemoryStream memoryStream = new MemoryStream((_product.Image as byte[])!);
            productPictureBox.Image = Image.FromStream(memoryStream);
        }
        else
            productPictureBox.Image = Image.FromFile(_product.Image.ToString()!);

        productPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

        ProductDetailsModalForm productDetails = new ProductDetailsModalForm(_product);
        productDetails.AddButtonClicked += (s, e) => AddButtonClicked?.Invoke(s, e);
        productDetails.DeleteButtonClicked += (s, e) => DeleteButtonClicked?.Invoke(s, e);

        productPictureBox.Click += (s, e) =>
        {
            productDetails.ShowDialog(this);
        };

        Tag = _product.Name;
    }
}