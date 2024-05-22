using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using System.Reflection;

namespace HardwareOnlineStore.MVP.Views.MainWindow.Sections;

public sealed partial class UserAccountControl : UserControl, IUserAccountView
{
    public event Func<Task<UserAccountModel?>> LoadUserData;
    public event Func<UserAccountModel, Task> UpdateData;

    public UserAccountControl()
        => InitializeComponent();

    void IView.Show()
        => Show();

    private async void LoadAsync(object sender, EventArgs e)
    {
        UserAccountModel? userAccount = await LoadUserData.Invoke()!;

        PropertyInfo[] userProperties = userAccount!.GetType().GetProperties();
        TextBox[] textBoxes = Controls.OfType<TextBox>().ToArray();

        foreach (PropertyInfo property in userProperties)
        {
            TextBox? textBox = textBoxes.FirstOrDefault(tb => tb.Name.Equals($"{property.Name}TextBox", StringComparison.CurrentCultureIgnoreCase));
            
            if (textBox != null)
                textBox.Text = property.GetValue(userAccount)!.ToString();
        }
    }

    private async void BtnUpdateData_ClickAsync(object sender, EventArgs e)
        => await UpdateData.Invoke(new UserAccountModel()
        {
            Name = nameTextBox.Text,
            SecondName = secondNameTextBox.Text,
            Patronymic = patronymicTextBox.Text,
            Gender = genderTextBox.Text,
            Age = ageTextBox.Text,
            Login = loginTextBox.Text,
            Password = passwordTextBox.Text,
            HouseNumber = houseNumberTextBox.Text,
            Street = streetTextBox.Text,
            City = cityTextBox.Text,
            Region = regionTextBox.Text,
            Country = countryTextBox.Text
        });

    void IView.ShowMessage(string message, string caption, MessageLevel level)
        => MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, level switch
        {
            MessageLevel.Info => MessageBoxIcon.Information,
            MessageLevel.Warning => MessageBoxIcon.Warning,
            MessageLevel.Error => MessageBoxIcon.Error,
            _ => MessageBoxIcon.None,
        });

    void IView.Close()
        => Parent?.Controls.Remove(this);
}