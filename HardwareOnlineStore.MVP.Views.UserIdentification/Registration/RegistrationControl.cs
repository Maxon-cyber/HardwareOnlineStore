using HardwareOnlineStore.MVP.ViewModels.UserIdentification;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using HardwareOnlineStore.MVP.Views.Abstractions.UserIdentification;

namespace HardwareOnlineStore.MVP.Views.UserIdentification.Registration;

public sealed partial class RegistrationControl : UserControl, IRegistrationView
{
    public event Func<RegistrationViewModel, Task> Registration;
    public event Action ReturnToAuthorization;

    public RegistrationControl()
       => InitializeComponent();

    public new void Show()
        => base.Show();

    public void ShowMessage(string message, string caption, MessageLevel level)
        => MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, level switch
        {
            MessageLevel.Info => MessageBoxIcon.Information,
            MessageLevel.Warning => MessageBoxIcon.Warning,
            MessageLevel.Error => MessageBoxIcon.Error,
            _ => MessageBoxIcon.None,
        });

    private async void BtnRegistration_Click(object sender, EventArgs e)
    {
        IEnumerable<TextBox> emptyTextBoxes = Controls.OfType<TextBox>().Where(tb => string.IsNullOrWhiteSpace(tb.Text));

        if (emptyTextBoxes.Any())
        {
            foreach (TextBox textBox in emptyTextBoxes)
                errorProvider.SetError(textBox, $"Введите данные {textBox.PlaceholderText}");

            return;
        }

        if (!manRadioButton.Checked && !womanRadioButton.Checked)
        {
            errorProvider.SetError(genderPanel, $"Выберите пол");
            return;
        }

        int age = Convert.ToInt32(ageTextBox.Text);
        if (age < 10 && age > 100)
        {
            errorProvider.SetError(ageTextBox, $"Возраст должен быть от 10 до 100 лет");
            return;
        }

        await Registration.Invoke(new RegistrationViewModel()
        {
            Name = nameTextBox.Text,
            SecondName = secondNameTextBox.Text,
            Patronymic = patronymicTextBox.Text,
            Gender = genderPanel.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked == true).Tag.ToString()!,
            Age = Convert.ToInt32(ageTextBox.Text),
            HouseNumber = houseNumberTextBox.Text,
            Street = streetTextBox.Text,
            City = cityTextBox.Text,
            Region = regionTextBox.Text,
            Country = countryTextBox.Text,
            Login = loginTextBox.Text,
            Password = passwordTextBox.Text,
        });
    }

    private void BtnReturnToAuthorization_Click(object sender, EventArgs e)
        => Close();

    private void FullnameTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            e.Handled = true;
    }

    private void AgeTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsDigit(e.KeyChar))
            e.Handled = true;
    }

    private void LoginTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!(char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
            e.Handled = true;
    }

    private void LocationTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (char.IsDigit(e.KeyChar) && e.KeyChar != ' ' && e.KeyChar != '.' && e.KeyChar != '/' && e.KeyChar != '\b' && e.KeyChar != '\r')
            e.Handled = true;
    }

    private void HouseNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != '/' && !char.IsControl(e.KeyChar))
            e.Handled = true;
    }

    public void Close()
        => Parent?.Controls.Remove(this);
}