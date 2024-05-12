namespace HardwareOnlineStore.MVP.ViewModels.UserIdentification;

public sealed class AuthorizationViewModel()
{
    public required string Login { get; init; }

    public required string Password { get; init; }
}