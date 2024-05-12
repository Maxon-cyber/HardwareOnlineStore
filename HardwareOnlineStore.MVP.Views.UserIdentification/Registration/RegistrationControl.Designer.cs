namespace HardwareOnlineStore.MVP.Views.UserIdentification.Registration;

public sealed partial class RegistrationControl
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
        nameTextBox = new TextBox();
        secondNameTextBox = new TextBox();
        patronymicTextBox = new TextBox();
        ageTextBox = new TextBox();
        loginTextBox = new TextBox();
        passwordTextBox = new TextBox();
        streetTextBox = new TextBox();
        houseNumberTextBox = new TextBox();
        cityTextBox = new TextBox();
        regionTextBox = new TextBox();
        registrationButton = new Button();
        returnToAuthorzationButton = new Button();
        countryTextBox = new TextBox();
        genderPanel = new Panel();
        womanRadioButton = new RadioButton();
        manRadioButton = new RadioButton();
        errorProvider = new ErrorProvider(components);
        genderPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // nameTextBox
        // 
        nameTextBox.Location = new Point(29, 101);
        nameTextBox.Name = "nameTextBox";
        nameTextBox.PlaceholderText = "Имя";
        nameTextBox.Size = new Size(100, 23);
        nameTextBox.TabIndex = 0;
        nameTextBox.KeyPress += FullnameTextBox_KeyPress;
        // 
        // secondNameTextBox
        // 
        secondNameTextBox.Location = new Point(29, 155);
        secondNameTextBox.Name = "secondNameTextBox";
        secondNameTextBox.PlaceholderText = "Фамилия";
        secondNameTextBox.Size = new Size(100, 23);
        secondNameTextBox.TabIndex = 1;
        secondNameTextBox.KeyPress += FullnameTextBox_KeyPress;
        // 
        // patronymicTextBox
        // 
        patronymicTextBox.Location = new Point(29, 207);
        patronymicTextBox.Name = "patronymicTextBox";
        patronymicTextBox.PlaceholderText = "Отчество";
        patronymicTextBox.Size = new Size(100, 23);
        patronymicTextBox.TabIndex = 2;
        patronymicTextBox.KeyPress += FullnameTextBox_KeyPress;
        // 
        // ageTextBox
        // 
        ageTextBox.Location = new Point(29, 260);
        ageTextBox.Name = "ageTextBox";
        ageTextBox.PlaceholderText = "Возраст";
        ageTextBox.Size = new Size(100, 23);
        ageTextBox.TabIndex = 3;
        ageTextBox.KeyPress += AgeTextBox_KeyPress;
        // 
        // loginTextBox
        // 
        loginTextBox.Location = new Point(188, 101);
        loginTextBox.Name = "loginTextBox";
        loginTextBox.PlaceholderText = "Логин";
        loginTextBox.Size = new Size(100, 23);
        loginTextBox.TabIndex = 6;
        loginTextBox.KeyPress += LoginTextBox_KeyPress;
        // 
        // passwordTextBox
        // 
        passwordTextBox.Location = new Point(188, 155);
        passwordTextBox.Name = "passwordTextBox";
        passwordTextBox.PlaceholderText = "Пароль";
        passwordTextBox.Size = new Size(100, 23);
        passwordTextBox.TabIndex = 7;
        // 
        // streetTextBox
        // 
        streetTextBox.Location = new Point(188, 207);
        streetTextBox.Name = "streetTextBox";
        streetTextBox.PlaceholderText = "Улица";
        streetTextBox.Size = new Size(100, 23);
        streetTextBox.TabIndex = 8;
        streetTextBox.KeyPress += LocationTextBox_KeyPress;
        // 
        // houseNumberTextBox
        // 
        houseNumberTextBox.Location = new Point(188, 260);
        houseNumberTextBox.Name = "houseNumberTextBox";
        houseNumberTextBox.PlaceholderText = "Номер дома";
        houseNumberTextBox.Size = new Size(100, 23);
        houseNumberTextBox.TabIndex = 9;
        houseNumberTextBox.KeyPress += HouseNumberTextBox_KeyPress;
        // 
        // cityTextBox
        // 
        cityTextBox.Location = new Point(188, 311);
        cityTextBox.Name = "cityTextBox";
        cityTextBox.PlaceholderText = "Город";
        cityTextBox.Size = new Size(100, 23);
        cityTextBox.TabIndex = 10;
        cityTextBox.KeyPress += LocationTextBox_KeyPress;
        // 
        // regionTextBox
        // 
        regionTextBox.Location = new Point(188, 363);
        regionTextBox.Name = "regionTextBox";
        regionTextBox.PlaceholderText = "Регион";
        regionTextBox.Size = new Size(100, 23);
        regionTextBox.TabIndex = 11;
        regionTextBox.KeyPress += LocationTextBox_KeyPress;
        // 
        // registrationButton
        // 
        registrationButton.Location = new Point(81, 489);
        registrationButton.Name = "registrationButton";
        registrationButton.Size = new Size(147, 23);
        registrationButton.TabIndex = 12;
        registrationButton.Text = "Регистрация";
        registrationButton.UseVisualStyleBackColor = true;
        registrationButton.Click += BtnRegistration_Click;
        // 
        // returnToAuthorzationButton
        // 
        returnToAuthorzationButton.Location = new Point(71, 518);
        returnToAuthorzationButton.Name = "returnToAuthorzationButton";
        returnToAuthorzationButton.Size = new Size(175, 23);
        returnToAuthorzationButton.TabIndex = 13;
        returnToAuthorzationButton.Text = "Вернусться к авторизации";
        returnToAuthorzationButton.UseVisualStyleBackColor = true;
        returnToAuthorzationButton.Click += BtnReturnToAuthorization_Click;
        // 
        // countryTextBox
        // 
        countryTextBox.Location = new Point(188, 414);
        countryTextBox.Name = "countryTextBox";
        countryTextBox.PlaceholderText = "Страна";
        countryTextBox.Size = new Size(100, 23);
        countryTextBox.TabIndex = 14;
        countryTextBox.KeyPress += LocationTextBox_KeyPress;
        // 
        // genderPanel
        // 
        genderPanel.Controls.Add(womanRadioButton);
        genderPanel.Controls.Add(manRadioButton);
        genderPanel.Location = new Point(18, 289);
        genderPanel.Name = "genderPanel";
        genderPanel.Size = new Size(153, 73);
        genderPanel.TabIndex = 15;
        // 
        // womanRadioButton
        // 
        womanRadioButton.AutoSize = true;
        womanRadioButton.Location = new Point(17, 38);
        womanRadioButton.Name = "womanRadioButton";
        womanRadioButton.Size = new Size(52, 19);
        womanRadioButton.TabIndex = 17;
        womanRadioButton.TabStop = true;
        womanRadioButton.Tag = "Woman";
        womanRadioButton.Text = "Жен.";
        womanRadioButton.UseVisualStyleBackColor = true;
        // 
        // manRadioButton
        // 
        manRadioButton.AutoSize = true;
        manRadioButton.Location = new Point(17, 13);
        manRadioButton.Name = "manRadioButton";
        manRadioButton.Size = new Size(54, 19);
        manRadioButton.TabIndex = 16;
        manRadioButton.TabStop = true;
        manRadioButton.Tag = "Man";
        manRadioButton.Text = "Муж.";
        manRadioButton.UseVisualStyleBackColor = true;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // RegistrationControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(genderPanel);
        Controls.Add(countryTextBox);
        Controls.Add(returnToAuthorzationButton);
        Controls.Add(registrationButton);
        Controls.Add(regionTextBox);
        Controls.Add(cityTextBox);
        Controls.Add(houseNumberTextBox);
        Controls.Add(streetTextBox);
        Controls.Add(passwordTextBox);
        Controls.Add(loginTextBox);
        Controls.Add(ageTextBox);
        Controls.Add(patronymicTextBox);
        Controls.Add(secondNameTextBox);
        Controls.Add(nameTextBox);
        Name = "RegistrationControl";
        Size = new Size(331, 576);
        KeyPress += LocationTextBox_KeyPress;
        genderPanel.ResumeLayout(false);
        genderPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion

    private TextBox nameTextBox;
    private TextBox secondNameTextBox;
    private TextBox patronymicTextBox;
    private TextBox ageTextBox;
    private TextBox loginTextBox;
    private TextBox passwordTextBox;
    private TextBox streetTextBox;
    private TextBox houseNumberTextBox;
    private TextBox cityTextBox;
    private TextBox regionTextBox;
    private Button registrationButton;
    private Button returnToAuthorzationButton;
    private TextBox countryTextBox;
    private Panel genderPanel;
    private ErrorProvider errorProvider;
    private System.ComponentModel.IContainer components;
    private RadioButton womanRadioButton;
    private RadioButton manRadioButton;
}