using System;
using System.Reactive;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Client.Desktop.Services.Authentication;
using Grpc.Core;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Authentication;

public class AuthenticationViewModel : ReactiveObject
{
    private readonly ITokenService _tokenService;
    private string _userName = string.Empty;
    private string _password = string.Empty;
    
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

    public AuthenticationViewModel(ITokenService tokenService)
    {
        _tokenService = tokenService;
        LoginCommand = ReactiveCommand.CreateFromTask(Login);
    }

    private async Task Login()
    {
        await _tokenService.Login(UserName, Password);
    }
}