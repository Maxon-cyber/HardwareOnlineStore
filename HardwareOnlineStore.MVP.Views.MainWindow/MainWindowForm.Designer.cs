namespace HardwareOnlineStore.MVP.Views.MainWindow;

public sealed partial class MainWindowForm
{
    private System.ComponentModel.IContainer _components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (_components != null))
            _components.Dispose();

        base.Dispose(disposing);
    }

    private void AddSection(UserControl control)
    {
        control.BorderStyle = BorderStyle.None;
        control.Dock = DockStyle.Fill;

        contentPanel.Controls.Add(control);
        contentPanel.Tag = control;

        control.BringToFront();
    }

    #region Windows Form Designer generated code
    private void InitializeComponent()
    {
        titlePanel = new Panel();
        sideBarPanel = new Panel();
        ordersButton = new Button();
        productsButton = new Button();
        accountButton = new Button();
        contentPanel = new Panel();
        sideBarPanel.SuspendLayout();
        SuspendLayout();
        // 
        // titlePanel
        // 
        titlePanel.Location = new Point(179, 0);
        titlePanel.Name = "titlePanel";
        titlePanel.Size = new Size(1063, 59);
        titlePanel.TabIndex = 0;
        // 
        // sideBarPanel
        // 
        sideBarPanel.Controls.Add(ordersButton);
        sideBarPanel.Controls.Add(productsButton);
        sideBarPanel.Controls.Add(accountButton);
        sideBarPanel.Location = new Point(-2, 0);
        sideBarPanel.Name = "sideBarPanel";
        sideBarPanel.Size = new Size(181, 662);
        sideBarPanel.TabIndex = 1;
        // 
        // ordersButton
        // 
        ordersButton.Location = new Point(0, 252);
        ordersButton.Name = "ordersButton";
        ordersButton.Size = new Size(177, 62);
        ordersButton.TabIndex = 2;
        ordersButton.Text = "Заказы";
        ordersButton.UseVisualStyleBackColor = true;
        ordersButton.Click += BtnOpenShoppingCart_Click;
        // 
        // productsButton
        // 
        productsButton.Location = new Point(0, 184);
        productsButton.Name = "productsButton";
        productsButton.Size = new Size(177, 70);
        productsButton.TabIndex = 1;
        productsButton.Text = "Продукты";
        productsButton.UseVisualStyleBackColor = true;
        productsButton.Click += BtnOpenProductShowcase_Click;
        // 
        // accountButton
        // 
        accountButton.Location = new Point(0, 125);
        accountButton.Name = "accountButton";
        accountButton.Size = new Size(177, 62);
        accountButton.TabIndex = 0;
        accountButton.Text = "Аккаунт";
        accountButton.UseVisualStyleBackColor = true;
        accountButton.Click += BtnOpenUserAccount_Click;
        // 
        // contentPanel
        // 
        contentPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        contentPanel.BackColor = SystemColors.Control;
        contentPanel.Location = new Point(179, 65);
        contentPanel.Name = "contentPanel";
        contentPanel.Size = new Size(1063, 600);
        contentPanel.TabIndex = 2;
        // 
        // MainWindowForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1242, 665);
        Controls.Add(titlePanel);
        Controls.Add(contentPanel);
        Controls.Add(sideBarPanel);
        Name = "MainWindowForm";
        Text = "MainWindowForm";
        sideBarPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
    #endregion

    private Panel titlePanel;
    private Panel sideBarPanel;
    private Button ordersButton;
    private Button productsButton;
    private Button accountButton;
    private Panel contentPanel;
}