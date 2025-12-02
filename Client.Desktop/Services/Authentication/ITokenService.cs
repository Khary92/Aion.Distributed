using System;
using System.Threading.Tasks;

namespace Client.Desktop.Services.Authentication;

public interface ITokenService
{
    event Func<string, Task>? Authenticated;
    bool IsAuthenticated { get; }
    Task<LoginResult> Login(string user, string pass);
    Task<LoginResult> Login2(string user, string pass);
    Task<string> GetToken();
}