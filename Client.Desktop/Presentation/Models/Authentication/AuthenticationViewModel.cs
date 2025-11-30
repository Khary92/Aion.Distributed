using System.Collections.Generic;
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
    private string _result = string.Empty;

    private readonly Dictionary<AuthenticationResult, string> _resultMessages = new()
    {
        { AuthenticationResult.Successful , "Successful" },
        { AuthenticationResult.InvalidCredentials, "Invalid credentials"},
        { AuthenticationResult.ServiceUnavailable , "Service unavailable"}
    };
    
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
    
    public string Result
    {
        get => _result;
        set => this.RaiseAndSetIfChanged(ref _result, value);
    }
    
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    private async Task Login()
    {
        var loginResult =  await _tokenService.Login(UserName, Password);
        Result = _resultMessages[loginResult];
    }
}