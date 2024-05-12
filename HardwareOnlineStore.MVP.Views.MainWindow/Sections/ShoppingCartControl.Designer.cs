namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections;

public sealed partial class ShoppingCartControl
{
    private System.ComponentModel.IContainer _components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (_components != null))
            _components.Dispose();

        base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором компонентов
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        viewProductsTLP = new TableLayoutPanel();
        searchButton = new Button();
        orderButton = new Button();
        searchTextBox = new TextBox();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // viewProductsTLP
        // 
        viewProductsTLP.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        viewProductsTLP.AutoScroll = true;
        viewProductsTLP.ColumnCount = 2;
        viewProductsTLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        viewProductsTLP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        viewProductsTLP.Location = new Point(0, 139);
        viewProductsTLP.Name = "viewProductsTLP";
        viewProductsTLP.RowCount = 2;
        viewProductsTLP.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        viewProductsTLP.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        viewProductsTLP.Size = new Size(864, 370);
        viewProductsTLP.TabIndex = 1;
        // 
        // searchButton
        // 
        searchButton.Location = new Point(440, 50);
        searchButton.Name = "searchButton";
        searchButton.Size = new Size(75, 23);
        searchButton.TabIndex = 2;
        searchButton.Text = "Поиск";
        searchButton.UseVisualStyleBackColor = true;
        searchButton.Click += BtnSearch_Click;
        // 
        // orderButton
        // 
        orderButton.Location = new Point(730, 25);
        orderButton.Name = "orderButton";
        orderButton.Size = new Size(107, 64);
        orderButton.TabIndex = 3;
        orderButton.Text = "Заказать";
        orderButton.UseVisualStyleBackColor = true;
        orderButton.Click += BtnOrder_Click;
        // 
        // searchTextBox
        // 
        searchTextBox.Location = new Point(29, 51);
        searchTextBox.Name = "searchTextBox";
        searchTextBox.PlaceholderText = "Поиск";
        searchTextBox.Size = new Size(385, 23);
        searchTextBox.TabIndex = 4;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // ShoppingCartControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(searchTextBox);
        Controls.Add(orderButton);
        Controls.Add(searchButton);
        Controls.Add(viewProductsTLP);
        Name = "ShoppingCartControl";
        Size = new Size(867, 512);
        Load += LoadOrderProducts;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion

    private TableLayoutPanel viewProductsTLP;
    private Button searchButton;
    private Button orderButton;
    private TextBox searchTextBox;
    private ErrorProvider errorProvider;
    private System.ComponentModel.IContainer components;
}