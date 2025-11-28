using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Service.Authorization.Persistence;
using Service.Authorization.Records;
using Service.Authorization.Service;

namespace Service.Authorization.Endpoints;

public class AuthoritaionEndpoint(IClientRepository clients, IUserRepository users, TokenService tokenService)
{
    public Task Handle(HttpRequest req)
    {
        var q = req.Query;
        var responseType = q["response_type"].ToString(); // must be "code"
        var clientId = q["client_id"].ToString();
        var redirectUri = q["redirect_uri"].ToString();
        var scope = q["scope"].ToString();
        var state = q["state"].ToString();
        var codeChallenge = q["code_challenge"].ToString();
        var codeChallengeMethod = q["code_challenge_method"].ToString();

        // Basic validation
        var client = clients.GetAll().FirstOrDefault(c => c.ClientId == clientId);
        if (client == null) return Task.FromResult(Results.BadRequest("unknown_client"));
        if (!client.RedirectUris.Contains(redirectUri)) return Task.FromResult(Results.BadRequest("invalid_redirect_uri"));
        if (responseType != "code") return Task.FromResult(Results.BadRequest("unsupported_response_type"));
        if (client.RequirePkce && string.IsNullOrEmpty(codeChallenge)) return Task.FromResult(Results.BadRequest("pkce_required"));

        // For demo: authenticate user by query (replace by real login UI)
        // e.g. /authorize?...&login_user=alice&login_pass=password
        var loginUser = q["login_user"].ToString();
        var loginPass = q["login_pass"].ToString();
        var user = users.GetAll().FirstOrDefault(u => u.UserId == loginUser && u.Password == loginPass);
        if (user == null)
            // return instructions in real app you'd show a login page
            return Task.FromResult(Results.Ok(new
            {
                msg = "Provide login_user & login_pass as query for demo. Example: login_user=alice&login_pass=password"
            }));

        // Create authorization code
        var code = Helpers.RandomString();
        tokenService.AuthCodes[code] = new AuthorizationCode
        {
            Code = code,
            ClientId = clientId,
            RedirectUri = redirectUri,
            UserId = user.UserId,
            CodeChallenge = codeChallenge,
            CodeChallengeMethod = codeChallengeMethod,
            Scopes = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries),
            Expiry = DateTime.UtcNow.AddMinutes(5)
        };

        var redirect = QueryHelpers.AddQueryString(redirectUri, new Dictionary<string, string>
        {
            ["code"] = code,
            ["state"] = state
        }!);

        return Task.FromResult(Results.Redirect(redirect));
    }
}