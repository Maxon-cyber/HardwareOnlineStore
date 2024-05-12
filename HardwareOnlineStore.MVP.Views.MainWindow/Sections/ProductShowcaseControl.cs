using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using HardwareOnlineStore.MVP.Views.MainWindow.Sections.ProductAndOrder;
using System.Collections.ObjectModel;

namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections;

public sealed partial class ProductShowcaseControl : UserControl, IProductShowcaseView
{
    public event Func<Task<ReadOnlyCollection<ProductModel>?>> LoadProducts;
    public event Func<ProductModel, Task> AddProduct;

    public ProductShowcaseControl()
        => InitializeComponent();

    public new void Show()
        => base.Show();

    private async void LoadAsync(object sender, EventArgs e)
    {
        ReadOnlyCollection<ProductModel> products = await LoadProducts.Invoke()!;

        viewProductsTLP.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

        ThreadPool.QueueUserWorkItem((_) =>
        {
            viewProductsTLP.Invoke(async () =>
            {
                int countOfProducts = products.Count;

                int columnTPL = viewProductsTLP.ColumnCount;
                int rowTPL = countOfProducts / columnTPL;
                viewProductsTLP.RowCount = rowTPL;

                viewProductsTLP.SuspendLayout();
                for (int index = 0; index < countOfProducts; index++)
                {
                    int column = index % columnTPL;
                    int row = index / columnTPL;

                    ProductModel currentProduct = products[index];

                    ProductControl productControl = new ProductControl(new Size(411, 179), currentProduct);
                    productControl.AddButtonClicked += async (s, e) => await AddProduct.Invoke(currentProduct);
                    await productControl.CreateProductViewAsync();

                    viewProductsTLP.Controls.Add(productControl, column, row);
                }
                viewProductsTLP.ResumeLayout();

                TableLayoutRowStyleCollection rowStyles = viewProductsTLP.RowStyles;
                TableLayoutColumnStyleCollection columnStyles = viewProductsTLP.ColumnStyles;

                rowStyles.Clear();
                columnStyles.Clear();

                for (int rowIndex = 0; rowIndex < rowTPL; rowIndex++)
                    rowStyles.Add(new RowStyle() { SizeType = SizeType.Percent });
                for (int columnIndex = 0; columnIndex < columnTPL; columnIndex++)
                    columnStyles.Add(new ColumnStyle() { SizeType = SizeType.Percent });

                viewProductsTLP.Invalidate();
            });
        });
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(searchTextBox.Text))
        {
            errorProvider.SetError(searchTextBox, "Введите название товара");
            return;
        }

        Control? needControl = viewProductsTLP.Controls.OfType<ProductControl>().Where(p => p.Tag == searchTextBox.Text).FirstOrDefault();

        if (needControl == null)
        {
            MessageBox.Show("Элемент не найден");
            return;
        }

        int desiredRow = viewProductsTLP.GetRow(needControl);
        int desiredColumn = viewProductsTLP.GetColumn(needControl);
        viewProductsTLP.ScrollControlIntoView(viewProductsTLP.GetControlFromPosition(desiredColumn, desiredRow));
        needControl.Select();
    }

    public void ShowMessage(string message, string caption, MessageLevel level)
        => MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, level switch
        {
            MessageLevel.Info => MessageBoxIcon.Information,
            MessageLevel.Warning => MessageBoxIcon.Warning,
            MessageLevel.Error => MessageBoxIcon.Error,
            _ => MessageBoxIcon.None,
        });

    public void Close()
       => Parent?.Controls.Remove(this);
}