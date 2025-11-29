using Microsoft.AspNetCore.WebUtilities;
using Service.Authorization.Persistence;
using Service.Authorization.Records;
using Service.Authorization.Service;

namespace Service.Authorization.Endpoints;

public class AuthoritaionEndpoint(IClientRepository clients, IUserRepository users, TokenService tokenService)
{
    public Task Handle(HttpRequest req, HttpResponse res)
    {
        var q = req.Query;
        var clientId = q["client_id"].ToString();
        var redirectUri = q["redirect_uri"].ToString();
        var state = q["state"].ToString();

        var client = clients.GetAll().FirstOrDefault(c => c.ClientId == clientId);
        if (client == null)
        {
            res.StatusCode = 400;
            return res.WriteAsync("unknown_client");
        }

        var loginUser = q["login_user"].ToString();
        var loginPass = q["login_pass"].ToString();
        var user = users.GetAll().FirstOrDefault(u => u.UserId == loginUser && u.Password == loginPass);

        if (user == null)
        {
            res.StatusCode = 200;
            return res.WriteAsync("Provide login_user & login_pass as query");
        }

        var code = Helpers.RandomString();
        tokenService.AuthCodes[code] = new AuthorizationCode
        {
            Code = code,
            ClientId = clientId,
            RedirectUri = redirectUri,
            UserId = user.UserId,
            Expiry = DateTime.UtcNow.AddMinutes(5)
        };

        var redirect = QueryHelpers.AddQueryString(redirectUri, new Dictionary<string, string>
        {
            ["code"] = code,
            ["state"] = state
        });

        res.StatusCode = StatusCodes.Status302Found;
        res.Headers.Location = redirect;

        return Task.CompletedTask;
    }
}