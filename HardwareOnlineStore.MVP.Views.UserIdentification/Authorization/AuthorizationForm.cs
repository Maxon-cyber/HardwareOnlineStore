using HardwareOnlineStore.MVP.ViewModels.UserIdentification;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using HardwareOnlineStore.MVP.Views.Abstractions.UserIdentification;
using HardwareOnlineStore.MVP.Views.UserIdentification.Registration;

namespace HardwareOnlineStore.MVP.Views.UserIdentification.Authorization;

public sealed partial class AuthorizationForm : Form, IAuthorizationView
{
    private bool _isRunable = false;
    private readonly ApplicationContext _context;

    public event Func<AuthorizationViewModel, Task> Authorization;
    public event Func<IRegistrationView> Registration;

    public AuthorizationForm(ApplicationContext context)
    {
        InitializeComponent();

        _context = context;
    }

    void IView.Show()
    {
        if (!_isRunable)
        {
            _isRunable = true;

            _context.MainForm = this;
            Application.Run(_context);
        }
    }

    private async void BtnLogin_Click(object sender, EventArgs e)
    {
        loginButton.Enabled = false;

        IEnumerable<TextBox> emptyTextBoxes = Controls.OfType<TextBox>().Where(tb => string.IsNullOrWhiteSpace(tb.Text));

        if (emptyTextBoxes.Any())
        {
            foreach (TextBox textBox in emptyTextBoxes)
                errorProvider.SetError(textBox, $"¬ведите значение {textBox.PlaceholderText}");

            return;
        }

        await Authorization.Invoke(new AuthorizationViewModel()
        {
            Login = loginTextBox.Text,
            Password = passwordTextBox.Text
        });

        loginButton.Enabled = true;
    }

    private void BtnRegistration_Click(object sender, EventArgs e)
    {
        IRegistrationView registration = Registration.Invoke();

        if (registration == null)
            return;

        RegistrationControl registrationControl = (registration as RegistrationControl)!;
        registrationControl.BorderStyle = BorderStyle.None;
        registrationControl.Dock = DockStyle.Fill;

        contentPanel.Controls.Add(registrationControl);
        contentPanel.Tag = registrationControl;

        registrationControl.BringToFront();
    }

    void IView.ShowMessage(string message, string caption, MessageLevel level)
         => MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, level switch
         {
             MessageLevel.Info => MessageBoxIcon.Information,
             MessageLevel.Warning => MessageBoxIcon.Warning,
             MessageLevel.Error => MessageBoxIcon.Error,
             _ => MessageBoxIcon.None,
         });

    void IView.Close()
        => base.Close();
}