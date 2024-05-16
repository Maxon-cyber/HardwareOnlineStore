namespace HardwareOnlineStore.MVP.Views.UserIdentification.Authorization;

public sealed partial class AuthorizationForm
{
    private System.ComponentModel.IContainer _components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (_components != null))
            _components.Dispose();        

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        contentPanel = new Panel();
        registrationButton = new Button();
        passwordTextBox = new TextBox();
        loginTextBox = new TextBox();
        loginButton = new Button();
        errorProvider = new ErrorProvider(components);
        contentPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // contentPanel
        // 
        contentPanel.Controls.Add(registrationButton);
        contentPanel.Controls.Add(passwordTextBox);
        contentPanel.Controls.Add(loginTextBox);
        contentPanel.Controls.Add(loginButton);
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Location = new Point(0, 0);
        contentPanel.Name = "contentPanel";
        contentPanel.Size = new Size(304, 541);
        contentPanel.TabIndex = 1;
        // 
        // registrationButton
        // 
        registrationButton.Location = new Point(82, 412);
        registrationButton.Name = "registrationButton";
        registrationButton.Size = new Size(138, 23);
        registrationButton.TabIndex = 3;
        registrationButton.Text = "Регистрация";
        registrationButton.UseVisualStyleBackColor = true;
        registrationButton.Click += BtnRegistration_Click;
        // 
        // passwordTextBox
        // 
        passwordTextBox.Location = new Point(96, 275);
        passwordTextBox.MaxLength = 15;
        passwordTextBox.Name = "passwordTextBox";
        passwordTextBox.PlaceholderText = "Пароль";
        passwordTextBox.Size = new Size(100, 23);
        passwordTextBox.TabIndex = 2;
        // 
        // loginTextBox
        // 
        loginTextBox.Location = new Point(96, 199);
        loginTextBox.MaxLength = 20;
        loginTextBox.Name = "loginTextBox";
        loginTextBox.PlaceholderText = "Логин";
        loginTextBox.Size = new Size(100, 23);
        loginTextBox.TabIndex = 1;
        // 
        // loginButton
        // 
        loginButton.Location = new Point(108, 351);
        loginButton.Name = "loginButton";
        loginButton.Size = new Size(75, 23);
        loginButton.TabIndex = 0;
        loginButton.Text = "Войти";
        loginButton.UseVisualStyleBackColor = true;
        loginButton.Click += BtnLogin_Click;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // AuthorizationForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(304, 541);
        Controls.Add(contentPanel);
        Name = "AuthorizationForm";
        Text = "Form1";
        contentPanel.ResumeLayout(false);
        contentPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }
    #endregion

    private Panel contentPanel;
    private TextBox passwordTextBox;
    private TextBox loginTextBox;
    private Button loginButton;
    private ErrorProvider errorProvider;
    private System.ComponentModel.IContainer components;
    private Button registrationButton;
}