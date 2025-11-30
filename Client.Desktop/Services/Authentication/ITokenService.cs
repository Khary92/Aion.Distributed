using System;
using System.Threading.Tasks;

namespace Client.Desktop.Services.Authentication;

public interface ITokenService
{
    event Func<string, Task>? Authenticated;
    bool IsAuthenticated { get; }
    Task<AuthenticationResult> Login(string user, string pass);
    Task<string> GetToken();
}