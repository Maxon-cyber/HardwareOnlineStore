using HardwareOnlineStore.MVP.ViewModels.MainWindow;

namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections.ProductAndOrder.Common;

public sealed partial class ProductDetailsModalForm : Form
{
    private readonly ProductModel _product;
    private readonly GrayOverlayForm _overlay;

    public event EventHandler? AddButtonClicked;
    public event EventHandler? DeleteButtonClicked;

    public ProductDetailsModalForm(ProductModel product)
    {
        InitializeComponent();

        _product = product;
        _overlay = new GrayOverlayForm();

        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
    }

    private sealed class GrayOverlayForm : Form
    {
        public GrayOverlayForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            //TopMost = true;
            BackColor = Color.Gray;
            Opacity = 0.5;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Location = Point.Empty;
        }
    }

    [Obsolete("Данная форма указывается в качестве модального окна. Запрещено использовать метод ProductDetailsModalWindow.Show().", true)]
    public new void Show()
        => throw new InvalidOperationException("Форма должна быть открыта как модальное окно.");

    [Obsolete("Данная форма указывается в качестве модального окна. Запрещено использовать метод ProductDetailsModalWindow.Show(IWin32Window owner).", true)]
    public new void Show(IWin32Window owner)
        => throw new InvalidOperationException("Форма должна быть открыта как модальное окно.");

    [Obsolete("Данная форма указывается в качестве модального окна. Запрещено использовать метод ProductDetailsModalWindow.ShowDialog().", true)]
    public new void ShowDialog()
        => throw new InvalidOperationException("Форма должна быть открыта как модальное окно.");

    public new DialogResult ShowDialog(IWin32Window owner)
    {
        ArgumentNullException.ThrowIfNull(owner);

        if (AddButtonClicked == null && DeleteButtonClicked == null)
            throw new ArgumentNullException("AddButtonClicked/DeleteButtonClicked", "Перед созданием представления продукта необходимо подписаться хотя бы на одно свойство: AddButtonClicked или DeleteButtonClicked");

        SuspendLayout();
        if (AddButtonClicked != null && DeleteButtonClicked == null)
        {
            Button addButton = new Button()
            {
                Name = "addButton",
                Text = "Добавить продукт",
                Size = new Size(259, 57),
                Location = new Point(0, 247)
            };

            addButton.Click += AddButtonClicked;

            contentPanel.Controls.Add(addButton);
        }
        else if (AddButtonClicked == null && DeleteButtonClicked != null)
        {
            Button deleteButton = new Button()
            {
                Name = "deleteButton",
                Text = "Удалить продукт",
                Size = new Size(259, 57),
                Location = new Point(0, 247)
            };

            deleteButton.Click += DeleteButtonClicked;

            contentPanel.Controls.Add(deleteButton);
        }

        nameLabelResult.Text = _product.Name;
        categoryLabelResult.Text = _product.Category;
        quantityLabelResult.Text = _product.Quantity.ToString();
        priceLabelResult.Text = _product.Price.ToString();

        Tag = _product.Name;

        ResumeLayout();

        _overlay.Show();

        return base.ShowDialog(owner);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);

        _overlay?.Close();
    }

    private void BtnClose_Click(object sender, EventArgs e)
        => Close();
}