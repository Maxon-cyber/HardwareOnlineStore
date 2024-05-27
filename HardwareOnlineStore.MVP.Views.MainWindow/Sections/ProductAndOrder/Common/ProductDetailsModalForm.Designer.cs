namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections.ProductAndOrder.Common
{
    partial class ProductDetailsModalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            contentPanel = new Panel();
            closeButton = new Button();
            priceLabelResult = new Label();
            quantityLabelResult = new Label();
            categoryLabelResult = new Label();
            nameLabelResult = new Label();
            priceLabel = new Label();
            quantityLabel = new Label();
            categoryLabel = new Label();
            nameLabel = new Label();
            contentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // contentPanel
            // 
            contentPanel.Controls.Add(closeButton);
            contentPanel.Controls.Add(priceLabelResult);
            contentPanel.Controls.Add(quantityLabelResult);
            contentPanel.Controls.Add(categoryLabelResult);
            contentPanel.Controls.Add(nameLabelResult);
            contentPanel.Controls.Add(priceLabel);
            contentPanel.Controls.Add(quantityLabel);
            contentPanel.Controls.Add(categoryLabel);
            contentPanel.Controls.Add(nameLabel);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(0, 0);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new Size(259, 307);
            contentPanel.TabIndex = 2;
            // 
            // closeButton
            // 
            closeButton.Location = new Point(181, 3);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(75, 23);
            closeButton.TabIndex = 9;
            closeButton.Text = "Закрыть";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += BtnClose_Click;
            // 
            // priceLabelResult
            // 
            priceLabelResult.AutoSize = true;
            priceLabelResult.Location = new Point(121, 136);
            priceLabelResult.Name = "priceLabelResult";
            priceLabelResult.Size = new Size(38, 15);
            priceLabelResult.TabIndex = 8;
            priceLabelResult.Text = "label8";
            // 
            // quantityLabelResult
            // 
            quantityLabelResult.AutoSize = true;
            quantityLabelResult.Location = new Point(121, 108);
            quantityLabelResult.Name = "quantityLabelResult";
            quantityLabelResult.Size = new Size(38, 15);
            quantityLabelResult.TabIndex = 7;
            quantityLabelResult.Text = "label7";
            // 
            // categoryLabelResult
            // 
            categoryLabelResult.AutoSize = true;
            categoryLabelResult.Location = new Point(121, 82);
            categoryLabelResult.Name = "categoryLabelResult";
            categoryLabelResult.Size = new Size(38, 15);
            categoryLabelResult.TabIndex = 6;
            categoryLabelResult.Text = "label6";
            // 
            // nameLabelResult
            // 
            nameLabelResult.AutoSize = true;
            nameLabelResult.Location = new Point(121, 55);
            nameLabelResult.Name = "nameLabelResult";
            nameLabelResult.Size = new Size(38, 15);
            nameLabelResult.TabIndex = 5;
            nameLabelResult.Text = "label5";
            // 
            // priceLabel
            // 
            priceLabel.AutoSize = true;
            priceLabel.Location = new Point(16, 136);
            priceLabel.Name = "priceLabel";
            priceLabel.Size = new Size(35, 15);
            priceLabel.TabIndex = 4;
            priceLabel.Text = "Цена";
            // 
            // quantityLabel
            // 
            quantityLabel.AutoSize = true;
            quantityLabel.Location = new Point(16, 108);
            quantityLabel.Name = "quantityLabel";
            quantityLabel.Size = new Size(72, 15);
            quantityLabel.TabIndex = 3;
            quantityLabel.Text = "Количество";
            // 
            // categoryLabel
            // 
            categoryLabel.AutoSize = true;
            categoryLabel.Location = new Point(16, 82);
            categoryLabel.Name = "categoryLabel";
            categoryLabel.Size = new Size(63, 15);
            categoryLabel.TabIndex = 2;
            categoryLabel.Text = "Категория";
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(16, 55);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(59, 15);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Название";
            // 
            // ProductDetailsModalForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(259, 307);
            ControlBox = false;
            Controls.Add(contentPanel);
            MaximumSize = new Size(275, 323);
            MinimumSize = new Size(275, 323);
            Name = "ProductDetailsModalForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel contentPanel;
        private Label priceLabelResult;
        private Label quantityLabelResult;
        private Label categoryLabelResult;
        private Label nameLabelResult;
        private Label priceLabel;
        private Label quantityLabel;
        private Label categoryLabel;
        private Label nameLabel;
        private Button closeButton;
    }
}