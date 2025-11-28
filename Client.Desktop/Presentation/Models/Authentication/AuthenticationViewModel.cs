using System.Reactive;
using System.Threading.Tasks;
using Client.Desktop.Services.Authentication;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Authentication;

public class AuthenticationViewModel : ReactiveObject
{
    private readonly ITokenService _tokenService;
    private string _password = string.Empty;
    private string _userName = string.Empty;

    public AuthenticationViewModel(ITokenService tokenService)
    {
        _tokenService = tokenService;
        LoginCommand = ReactiveCommand.CreateFromTask(Login);
    }

    public string UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public string AccessToken { get; private set; } = string.Empty;
    public string RefreshToken { get; private set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    private async Task Login()
    {
        await _tokenService.Login(UserName, Password);
    }
}