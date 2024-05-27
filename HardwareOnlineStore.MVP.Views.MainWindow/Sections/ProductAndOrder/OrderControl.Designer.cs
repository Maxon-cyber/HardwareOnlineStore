namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections.ProductAndOrder;

public sealed partial class OrderControl
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
        tableLayoutPanel1 = new TableLayoutPanel();
        panel1 = new Panel();
        pictureBox1 = new PictureBox();
        label1 = new Label();
        label2 = new Label();
        label3 = new Label();
        label4 = new Label();
        label5 = new Label();
        label6 = new Label();
        label7 = new Label();
        label8 = new Label();
        button1 = new Button();
        tableLayoutPanel1.SuspendLayout();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(panel1, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Size = new Size(520, 569);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // panel1
        // 
        panel1.Controls.Add(button1);
        panel1.Controls.Add(label8);
        panel1.Controls.Add(label7);
        panel1.Controls.Add(label6);
        panel1.Controls.Add(label5);
        panel1.Controls.Add(label4);
        panel1.Controls.Add(label3);
        panel1.Controls.Add(label2);
        panel1.Controls.Add(label1);
        panel1.Controls.Add(pictureBox1);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(3, 3);
        panel1.Name = "panel1";
        panel1.Size = new Size(254, 278);
        panel1.TabIndex = 0;
        // 
        // pictureBox1
        // 
        pictureBox1.Location = new Point(3, 3);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(248, 142);
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(3, 148);
        label1.Name = "label1";
        label1.Size = new Size(38, 15);
        label1.TabIndex = 1;
        label1.Text = "label1";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(3, 172);
        label2.Name = "label2";
        label2.Size = new Size(38, 15);
        label2.TabIndex = 2;
        label2.Text = "label2";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(3, 196);
        label3.Name = "label3";
        label3.Size = new Size(38, 15);
        label3.TabIndex = 3;
        label3.Text = "label3";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(3, 221);
        label4.Name = "label4";
        label4.Size = new Size(38, 15);
        label4.TabIndex = 4;
        label4.Text = "label4";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(108, 148);
        label5.Name = "label5";
        label5.Size = new Size(38, 15);
        label5.TabIndex = 5;
        label5.Text = "label5";
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new Point(108, 172);
        label6.Name = "label6";
        label6.Size = new Size(38, 15);
        label6.TabIndex = 6;
        label6.Text = "label6";
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new Point(108, 196);
        label7.Name = "label7";
        label7.Size = new Size(38, 15);
        label7.TabIndex = 7;
        label7.Text = "label7";
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Location = new Point(108, 221);
        label8.Name = "label8";
        label8.Size = new Size(38, 15);
        label8.TabIndex = 8;
        label8.Text = "label8";
        // 
        // button1
        // 
        button1.Location = new Point(0, 239);
        button1.Name = "button1";
        button1.Size = new Size(254, 39);
        button1.TabIndex = 9;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        // 
        // OrderControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel1);
        Name = "OrderControl";
        Size = new Size(520, 569);
        tableLayoutPanel1.ResumeLayout(false);
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
    }
    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel1;
    private Label label3;
    private Label label2;
    private Label label1;
    private PictureBox pictureBox1;
    private Label label4;
    private Button button1;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
}