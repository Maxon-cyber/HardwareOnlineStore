using HardwareOnlineStore.MVP.ViewModels.MainWindow;

namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections.ProductAndOrder;

public sealed partial class ProductControl : UserControl
{
    private readonly ProductModel _product;

    public event EventHandler AddButtonClicked;
    public event EventHandler DeleteButtonClicked;

    public ProductControl(Size size, ProductModel product)
    {
        InitializeComponent();

        Size = size;
        _product = product;
    }

    public async Task CreateProductViewAsync()
    {
        if (AddButtonClicked == null && DeleteButtonClicked == null)
            throw new ArgumentNullException("Перед созданием представления продукта необходимо подписаться хотя бы на одно свойство: AddButtonClicked или DeleteButtonClicked");

        await using MemoryStream memoryStream = new MemoryStream(_product.Image);

        productPictureBox.Name = $"{_product.Name}PictureBox";
        productPictureBox.Image = Image.FromStream(memoryStream);
        productPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

        SuspendLayout();
        if (AddButtonClicked != null && DeleteButtonClicked == null)
        {
            Button addButton = new Button()
            {
                Name = "addButton",
                Text = "Добавить продукт",
                Size = new Size(405, 45),
                Location = new Point(3, 131)
            };

            addButton.Click += (s, e) => { AddButtonClicked?.Invoke(this, EventArgs.Empty); };

            contentProductPanel.Controls.Add(addButton);
        }
        else if (AddButtonClicked == null && DeleteButtonClicked != null)
        {
            Button deleteButton = new Button()
            {
                Name = "deleteButton",
                Text = "Удалить продукт",
                Size = new Size(405, 45),
                Location = new Point(3, 131)
            };

            deleteButton.Click += (s, e) => { DeleteButtonClicked?.Invoke(this, EventArgs.Empty); };

            contentProductPanel.Controls.Add(deleteButton);
        }
        else if(AddButtonClicked != null && DeleteButtonClicked != null)
        {
            Button addButton = new Button()
            {
                Name = "addButton",
                Text = "Добавить продукт",
                Size = new Size(114, 45),
                Location = new Point(3, 131)
            };

            Button deleteButton = new Button()
            {
                Name = "deleteButton",
                Text = "Удалить продукт",
                Size = new Size(114, 45),
                Location = new Point(294, 131)
            };

            addButton.Click += (s, e) => { AddButtonClicked?.Invoke(this, EventArgs.Empty); };
            deleteButton.Click += (s, e) => { DeleteButtonClicked?.Invoke(this, EventArgs.Empty); };

            contentProductPanel.Controls.Add(addButton);
            contentProductPanel.Controls.Add(deleteButton);
        }
        ResumeLayout();

        Invalidate();

        Refresh();

        Tag = _product.Name;
    }
}