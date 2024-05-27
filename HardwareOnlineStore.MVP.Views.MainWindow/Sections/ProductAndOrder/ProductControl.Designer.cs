namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections.ProductAndOrder;

public sealed partial class ProductControl
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
        contentProductPanel = new Panel();
        productPictureBox = new PictureBox();
        contentProductPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)productPictureBox).BeginInit();
        SuspendLayout();
        // 
        // contentProductPanel
        // 
        contentProductPanel.Controls.Add(productPictureBox);
        contentProductPanel.Dock = DockStyle.Fill;
        contentProductPanel.Location = new Point(0, 0);
        contentProductPanel.Name = "contentProductPanel";
        contentProductPanel.Size = new Size(516, 248);
        contentProductPanel.TabIndex = 0;
        // 
        // productPictureBox
        // 
        productPictureBox.Location = new Point(0, 0);
        productPictureBox.Name = "productPictureBox";
        productPictureBox.Size = new Size(516, 248);
        productPictureBox.TabIndex = 0;
        productPictureBox.TabStop = false;
        // 
        // ProductControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(contentProductPanel);
        Name = "ProductControl";
        Size = new Size(516, 248);
        contentProductPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)productPictureBox).EndInit();
        ResumeLayout(false);
    }
    #endregion

    private Panel contentProductPanel;
    private PictureBox productPictureBox;
}