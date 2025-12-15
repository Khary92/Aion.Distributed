using System;
using System.Threading.Tasks;
using OpenIddict.Client;

namespace Client.Desktop.Services.Authentication;

public class TokenService(OpenIddictClientService openIddictClientService) : ITokenService
{
    private string _accessToken = string.Empty;
    private string _user = string.Empty;
    private string _password = string.Empty;
    private DateTimeOffset? _expiryTime = DateTimeOffset.MinValue;

    public event Func<string, Task>? Authenticated;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken);

    public async Task<string> GetToken()
    {
        if (_expiryTime == null || _expiryTime < DateTimeOffset.UtcNow.AddMinutes(-10))
        {
            await Login(_user, _password);
        }

        return _accessToken;
    }

    public async Task<LoginResult> Login(string user, string pass)
    {
        _user = user;
        _password = pass;

        var request = new OpenIddictClientModels.PasswordAuthenticationRequest
        {
            Username = user,
            Password = pass,
            Scopes = ["openid"]
        };

        var result = await openIddictClientService.AuthenticateWithPasswordAsync(request);

        _accessToken = result.AccessToken;
        _expiryTime = result.AccessTokenExpirationDate;

        if (Authenticated == null) throw new InvalidOperationException("No forwarding receiver set");

        await Authenticated.Invoke(_accessToken);
        return LoginResult.Successful;
    }
}