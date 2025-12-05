using System;
using System.Threading.Tasks;
using OpenIddict.Client;

namespace Client.Desktop.Services.Authentication;

public class TokenService(OpenIddictClientService openIddictClientService) : ITokenService
{
    private string _accessToken = string.Empty;
    private string _user = string.Empty;
    private string _password = string.Empty;
    
    public event Func<string, Task>? Authenticated;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken);

    public async Task<string> GetToken()
    {
        //TODO save expiry time and request new token if expired
        if (!IsAuthenticated)
        {
            await Login(_user, _password);
        }
        
        return _accessToken;
    }
    
    public async Task<LoginResult> Login(string user, string pass)
    {
        _user = user;
        _password = pass;
        
        var result = await openIddictClientService.AuthenticateWithClientCredentialsAsync(new());

        if (result.TokenResponse.Error != null) return LoginResult.InvalidCredentials;

        _accessToken = result.AccessToken;

        if (Authenticated == null) throw new InvalidOperationException("No forwarding receiver set");

        await Authenticated.Invoke(_accessToken);
        return LoginResult.Successful;
    }
}