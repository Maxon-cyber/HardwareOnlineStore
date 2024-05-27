using HardwareOnlineStore.Core.Common.Abstractions;
using HardwareOnlineStore.Entities.User;
using HardwareOnlineStore.MVP.Presenters.Contracts;
using HardwareOnlineStore.MVP.ViewModels.MainWindow;
using HardwareOnlineStore.MVP.Views.Abstractions.MainWindow.Sections;
using HardwareOnlineStore.MVP.Views.Abstractions.Shared;
using HardwareOnlineStore.Services.Entity.Contracts;
using HardwareOnlineStore.Services.Entity.SqlServerService;
using HardwareOnlineStore.Services.Entity.SqlServerService.DataProcessing;
using HardwareOnlineStore.Services.Utilities.Caching.Memory;
using System.Text;

namespace HardwareOnlineStore.MVP.Presenters.MainWindow.Sections.UserAccount;

public sealed class UserAccountPresenter : Presenter<IUserAccountView>
{
    private readonly UserService _service;
    private readonly MemoryCache<UserEntity> _memoryCache;

    public UserAccountPresenter(IApplicationController controller, IUserAccountView view, SqlServerService service)
        : base(controller, view)
    {
        _memoryCache = MemoryCache<UserEntity>.Instance;
        _service = service.User;

        View.LoadUserData += LoadUserDataAsync;
        View.UpdateData += UpdateDataAsync;
    }

    private async Task<UserAccountModel?> LoadUserDataAsync()
    {
        UserEntity? user = await _memoryCache.ReadByKeyAsync("User");

        if (user == null)
        {
            View.ShowMessage("Не удалось загрузить данные", "Ой...", MessageLevel.Error);
            return await Task.FromResult<UserAccountModel?>(null);
        }

        UserAccountModel userAccount = new UserAccountModel()
        {
            Name = user.Name,
            SecondName = user.SecondName,
            Patronymic = user.Patronymic,
            Gender = user.Gender.ToString(),
            Age = user.Age.ToString(),
            Login = user.Login,
            Password = Encoding.UTF8.GetString(user.Password),
            HouseNumber = user.Location.HouseNumber,
            Street = user.Location.Street,
            City = user.Location.City,
            Region = user.Location.Region,
            Country = user.Location.Country
        };

        return userAccount;
    }

    private async Task UpdateDataAsync(UserAccountModel model)
    {
        bool result = await _service.ChangeUserAsync(TypeOfUpdateCommand.Insert, new UserEntity()
        {
            Name = model.Name,
            SecondName = model.SecondName,
            Patronymic = model.Patronymic,
            Gender = Enum.Parse<Gender>(model.Gender),
            Age = Convert.ToUInt32(model.Age),
            Login = model.Login,
            Password = Encoding.UTF8.GetBytes(model.Password),
            Location = new Location()
            {
                HouseNumber = model.HouseNumber,
                Street = model.Street,
                City = model.City,
                Region = model.Region,
                Country = model.Country
            },
        });

        if (result)
            View.ShowMessage("Не удалось обновить данные", "Ой...", MessageLevel.Error);
        else
            View.ShowMessage("Данные успешно обновлены", "Успех", MessageLevel.Info);
    }
}