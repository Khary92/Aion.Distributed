using Microsoft.AspNetCore.WebUtilities;
using Service.Authorization.Persistence;
using Service.Authorization.Records;
using Service.Authorization.Service;

namespace Service.Authorization.Endpoints;

public class AuthorizationEndpoint(IClientRepository clients, IUserRepository users, TokenService tokenService)
{
    public Task<IResult> Handle(HttpContext context)
    {
        var q = context.Request.Query;
        var clientId = q["client_id"].ToString();
        var redirectUri = q["redirect_uri"].ToString();
        var state = q["state"].ToString();

        var client = clients.GetAll().FirstOrDefault(c => c.ClientId == clientId);
        if (client == null)
            return Task.FromResult(Results.BadRequest("unknown_client"));

        var loginUser = q["login_user"].ToString();
        var loginPass = q["login_pass"].ToString();
        var user = users.GetAll().FirstOrDefault(u => u.UserId == loginUser && u.Password == loginPass);
        if (user == null)
            return Task.FromResult(Results.Ok("Provide login_user & login_pass as query"));
        
        var codeChallenge = q["code_challenge"].ToString();
        var codeChallengeMethod = q["code_challenge_method"].ToString();
        
        var code = Helpers.Helpers.RandomString();
        tokenService.AuthCodes[code] = new AuthorizationCode(
            code,
            client.ClientId,
            redirectUri,
            user.UserId,
            codeChallenge,
            codeChallengeMethod,
            [], // Scopes
            DateTime.UtcNow.AddMinutes(5)
        );

        var redirect = QueryHelpers.AddQueryString(redirectUri, new Dictionary<string, string?>
        {
            ["code"] = code,
            ["state"] = state
        });
        
        return Task.FromResult(Results.Redirect(redirect));
    }

}