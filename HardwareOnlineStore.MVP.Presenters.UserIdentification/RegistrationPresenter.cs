using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Entities.User;
using HardwareOnlineStore.MVP.Presenters.Contracts;
using HardwareOnlineStore.MVP.ViewModels.UserIdentification;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using HardwareOnlineStore.MVP.Views.Abstractions.UserIdentification;
using HardwareOnlineStore.Services.Entity.Contracts.Abstractions;
using HardwareOnlineStore.Services.Entity.SqlServerService;
using HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;
using System.Text;

namespace HardwareOnlineStore.MVP.Presenters.UserIdentification;

public sealed class RegistrationPresenter : Presenter<IRegistrationView>
{
    private readonly UserService _userService;

    public RegistrationPresenter(IApplicationController controller, IRegistrationView view, SqlServerService service)
        : base(controller, view)
    {
        _userService = service.User;

        View.Registration += RegistrationAsync;
        View.ReturnToAuthorization += ReturnToAuthorization;
    }

    private async Task RegistrationAsync(RegistrationViewModel model)
    {
        object? isAdded = await _userService.ChangeUserAsync(TypeOfUpdateCommand.Insert, new UserEntity()
        {
            Name = model.Name,
            SecondName = model.SecondName,
            Patronymic = model.Patronymic,
            Gender = Enum.Parse<Gender>(model.Gender),
            Age = Convert.ToUInt32(model.Age),
            Location = new Location()
            {
                HouseNumber = model.HouseNumber,
                Street = model.Street,
                City = model.City,
                Region = model.Region,
                Country = model.Country
            },
            Login = model.Login,
            Password = Encoding.UTF8.GetBytes(model.Password),
            Role = UserParameters.DEFAULT_ROLE
        });

        if (!Convert.ToBoolean(isAdded))
            View.ShowMessage("Пользователь с таким логином уже зарегистрирован", "Предупреждение", MessageLevel.Warning);
        else
        {
            View.ShowMessage("Вы успешно зарегистрированы", "Успех", MessageLevel.Info);
            View.Close();
        }
    }

    private void ReturnToAuthorization()
        => View.Close();
}