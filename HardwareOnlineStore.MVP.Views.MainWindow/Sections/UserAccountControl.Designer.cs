namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections;

public sealed partial class UserAccountControl
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
        nameTextBox = new TextBox();
        secondNameTextBox = new TextBox();
        genderTextBox = new TextBox();
        passwordTextBox = new TextBox();
        ageTextBox = new TextBox();
        loginTextBox = new TextBox();
        patronymicTextBox = new TextBox();
        cityTextBox = new TextBox();
        houseNumberTextBox = new TextBox();
        streetTextBox = new TextBox();
        regionTextBox = new TextBox();
        countryTextBox = new TextBox();
        updateDataButton = new Button();
        nameLabel = new Label();
        label2 = new Label();
        label3 = new Label();
        label4 = new Label();
        label5 = new Label();
        label6 = new Label();
        label7 = new Label();
        label8 = new Label();
        label9 = new Label();
        label10 = new Label();
        label11 = new Label();
        label12 = new Label();
        userPhotoPictureBox = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)userPhotoPictureBox).BeginInit();
        SuspendLayout();
        // 
        // nameTextBox
        // 
        nameTextBox.Location = new Point(46, 165);
        nameTextBox.Name = "nameTextBox";
        nameTextBox.Size = new Size(100, 23);
        nameTextBox.TabIndex = 0;
        // 
        // secondNameTextBox
        // 
        secondNameTextBox.Location = new Point(46, 231);
        secondNameTextBox.Name = "secondNameTextBox";
        secondNameTextBox.Size = new Size(100, 23);
        secondNameTextBox.TabIndex = 1;
        // 
        // genderTextBox
        // 
        genderTextBox.Location = new Point(188, 165);
        genderTextBox.Name = "genderTextBox";
        genderTextBox.Size = new Size(100, 23);
        genderTextBox.TabIndex = 2;
        // 
        // passwordTextBox
        // 
        passwordTextBox.Location = new Point(329, 165);
        passwordTextBox.Name = "passwordTextBox";
        passwordTextBox.Size = new Size(100, 23);
        passwordTextBox.TabIndex = 3;
        // 
        // ageTextBox
        // 
        ageTextBox.Location = new Point(188, 231);
        ageTextBox.Name = "ageTextBox";
        ageTextBox.Size = new Size(100, 23);
        ageTextBox.TabIndex = 4;
        // 
        // loginTextBox
        // 
        loginTextBox.Location = new Point(188, 298);
        loginTextBox.Name = "loginTextBox";
        loginTextBox.Size = new Size(100, 23);
        loginTextBox.TabIndex = 5;
        // 
        // patronymicTextBox
        // 
        patronymicTextBox.Location = new Point(46, 298);
        patronymicTextBox.Name = "patronymicTextBox";
        patronymicTextBox.Size = new Size(100, 23);
        patronymicTextBox.TabIndex = 6;
        // 
        // cityTextBox
        // 
        cityTextBox.Location = new Point(469, 165);
        cityTextBox.Name = "cityTextBox";
        cityTextBox.Size = new Size(100, 23);
        cityTextBox.TabIndex = 7;
        // 
        // houseNumberTextBox
        // 
        houseNumberTextBox.Location = new Point(329, 231);
        houseNumberTextBox.Name = "houseNumberTextBox";
        houseNumberTextBox.Size = new Size(100, 23);
        houseNumberTextBox.TabIndex = 8;
        // 
        // streetTextBox
        // 
        streetTextBox.Location = new Point(329, 298);
        streetTextBox.Name = "streetTextBox";
        streetTextBox.Size = new Size(100, 23);
        streetTextBox.TabIndex = 9;
        // 
        // regionTextBox
        // 
        regionTextBox.Location = new Point(469, 231);
        regionTextBox.Name = "regionTextBox";
        regionTextBox.Size = new Size(100, 23);
        regionTextBox.TabIndex = 10;
        // 
        // countryTextBox
        // 
        countryTextBox.Location = new Point(469, 298);
        countryTextBox.Name = "countryTextBox";
        countryTextBox.Size = new Size(100, 23);
        countryTextBox.TabIndex = 11;
        // 
        // updateDataButton
        // 
        updateDataButton.Location = new Point(264, 368);
        updateDataButton.Name = "updateDataButton";
        updateDataButton.Size = new Size(75, 23);
        updateDataButton.TabIndex = 12;
        updateDataButton.Text = "Обновить данные";
        updateDataButton.UseVisualStyleBackColor = true;
        updateDataButton.Click += BtnUpdateData_ClickAsync;
        // 
        // nameLabel
        // 
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(46, 147);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(31, 15);
        nameLabel.TabIndex = 13;
        nameLabel.Text = "Имя";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(46, 213);
        label2.Name = "label2";
        label2.Size = new Size(58, 15);
        label2.TabIndex = 14;
        label2.Text = "Фамилия";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(46, 280);
        label3.Name = "label3";
        label3.Size = new Size(58, 15);
        label3.TabIndex = 15;
        label3.Text = "Отчество";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(188, 147);
        label4.Name = "label4";
        label4.Size = new Size(30, 15);
        label4.TabIndex = 16;
        label4.Text = "Пол";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(188, 213);
        label5.Name = "label5";
        label5.Size = new Size(50, 15);
        label5.TabIndex = 17;
        label5.Text = "Возраст";
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new Point(188, 280);
        label6.Name = "label6";
        label6.Size = new Size(41, 15);
        label6.TabIndex = 18;
        label6.Text = "Логин";
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new Point(330, 147);
        label7.Name = "label7";
        label7.Size = new Size(49, 15);
        label7.TabIndex = 19;
        label7.Text = "Пароль";
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Location = new Point(330, 213);
        label8.Name = "label8";
        label8.Size = new Size(76, 15);
        label8.TabIndex = 20;
        label8.Text = "Номер дома";
        // 
        // label9
        // 
        label9.AutoSize = true;
        label9.Location = new Point(330, 280);
        label9.Name = "label9";
        label9.Size = new Size(41, 15);
        label9.TabIndex = 21;
        label9.Text = "Улица";
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.Location = new Point(469, 147);
        label10.Name = "label10";
        label10.Size = new Size(40, 15);
        label10.TabIndex = 22;
        label10.Text = "Город";
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Location = new Point(469, 213);
        label11.Name = "label11";
        label11.Size = new Size(46, 15);
        label11.TabIndex = 23;
        label11.Text = "Регион";
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Location = new Point(469, 280);
        label12.Name = "label12";
        label12.Size = new Size(46, 15);
        label12.TabIndex = 24;
        label12.Text = "Страна";
        // 
        // userPhotoPictureBox
        // 
        userPhotoPictureBox.Location = new Point(198, 15);
        userPhotoPictureBox.Name = "userPhotoPictureBox";
        userPhotoPictureBox.Size = new Size(190, 104);
        userPhotoPictureBox.TabIndex = 25;
        userPhotoPictureBox.TabStop = false;
        // 
        // UserAccountControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(userPhotoPictureBox);
        Controls.Add(label12);
        Controls.Add(label11);
        Controls.Add(label10);
        Controls.Add(label9);
        Controls.Add(label8);
        Controls.Add(label7);
        Controls.Add(label6);
        Controls.Add(label5);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(nameLabel);
        Controls.Add(updateDataButton);
        Controls.Add(countryTextBox);
        Controls.Add(regionTextBox);
        Controls.Add(streetTextBox);
        Controls.Add(houseNumberTextBox);
        Controls.Add(cityTextBox);
        Controls.Add(patronymicTextBox);
        Controls.Add(loginTextBox);
        Controls.Add(ageTextBox);
        Controls.Add(passwordTextBox);
        Controls.Add(genderTextBox);
        Controls.Add(secondNameTextBox);
        Controls.Add(nameTextBox);
        Name = "UserAccountControl";
        Size = new Size(618, 427);
        Load += LoadAsync;
        ((System.ComponentModel.ISupportInitialize)userPhotoPictureBox).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion

    private TextBox nameTextBox;
    private TextBox secondNameTextBox;
    private TextBox genderTextBox;
    private TextBox passwordTextBox;
    private TextBox ageTextBox;
    private TextBox loginTextBox;
    private TextBox patronymicTextBox;
    private TextBox cityTextBox;
    private TextBox houseNumberTextBox;
    private TextBox streetTextBox;
    private TextBox regionTextBox;
    private TextBox countryTextBox;
    private Button updateDataButton;
    private Label nameLabel;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private Label label10;
    private Label label11;
    private Label label12;
    private PictureBox userPhotoPictureBox;
}