using System;
using System.Threading.Tasks;

namespace Client.Desktop.Services.Authentication;

public interface ITokenService
{
    Task<string> GetToken();
    bool IsAuthenticated { get; }
    event Func<string, Task>? Authenticated;
    Task<LoginResult> Login(string user, string pass);
}