namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections;

public sealed partial class ProductShowcaseControl
{
    private System.ComponentModel.IContainer _components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (_components != null))
            _components.Dispose();

        base.Dispose(disposing);
    }

    #region Component Designer generated code
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        viewProductsTLP = new TableLayoutPanel();
        searchTextBox = new TextBox();
        searchButton = new Button();
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
        viewProductsTLP.Location = new Point(0, 142);
        viewProductsTLP.Name = "viewProductsTLP";
        viewProductsTLP.RowCount = 2;
        viewProductsTLP.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        viewProductsTLP.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        viewProductsTLP.Size = new Size(834, 370);
        viewProductsTLP.TabIndex = 0;
        // 
        // searchTextBox
        // 
        searchTextBox.Location = new Point(40, 56);
        searchTextBox.Name = "searchTextBox";
        searchTextBox.PlaceholderText = "Поиск";
        searchTextBox.Size = new Size(451, 23);
        searchTextBox.TabIndex = 1;
        // 
        // searchButton
        // 
        searchButton.Location = new Point(538, 56);
        searchButton.Name = "searchButton";
        searchButton.Size = new Size(75, 23);
        searchButton.TabIndex = 2;
        searchButton.Text = "Поиск";
        searchButton.UseVisualStyleBackColor = true;
        searchButton.Click += BtnSearch_Click;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // ProductShowcaseControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.Control;
        Controls.Add(searchButton);
        Controls.Add(searchTextBox);
        Controls.Add(viewProductsTLP);
        Name = "ProductShowcaseControl";
        Size = new Size(834, 512);
        Load += LoadAsync;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion

    private TableLayoutPanel viewProductsTLP;
    private TextBox searchTextBox;
    private Button searchButton;
    private ErrorProvider errorProvider;
    private System.ComponentModel.IContainer components;
}