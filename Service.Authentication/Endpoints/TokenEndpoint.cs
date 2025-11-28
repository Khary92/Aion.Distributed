using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Service.Authorization.Persistence;
using Service.Authorization.Records;
using Service.Authorization.Service;

namespace Service.Authorization.Endpoints;

public class TokenEndpoint(IUserRepository users, TokenService tokenService)
{
    public async Task<IResult> Handle(HttpRequest req)
    {
        if (!req.HasFormContentType) return Results.BadRequest("invalid_request");
        var form = await req.ReadFormAsync();
        var grantType = form["grant_type"].ToString();

        if (grantType == "authorization_code")
        {
            var code = form["code"].ToString();
            var redirectUri = form["redirect_uri"].ToString();
            var clientId = form["client_id"].ToString();
            var codeVerifier = form["code_verifier"].ToString();

            if (!tokenService.AuthCodes.TryGetValue(code, out var stored))
                return Results.BadRequest(new { error = "invalid_grant" });
            if (stored.Expiry < DateTime.UtcNow)
            {
                tokenService.AuthCodes.Remove(code);
                return Results.BadRequest(new { error = "invalid_grant" });
            }

            if (stored.ClientId != clientId) return Results.BadRequest(new { error = "invalid_grant" });
            if (stored.RedirectUri != redirectUri) return Results.BadRequest(new { error = "invalid_grant" });

            // Verify PKCE if required
            if (!Helpers.VerifyPkce(stored.CodeChallenge, stored.CodeChallengeMethod, codeVerifier))
                return Results.BadRequest(new { error = "invalid_grant", error_description = "pkce_failed" });

            // One-time use
            tokenService.AuthCodes.Remove(code);

            // Issue tokens
            var user = users.GetAll().First(u => u.UserId == stored.UserId);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId),
                new("name", user.Name ?? user.UserId)
            };
            var accessToken = tokenService.CreateJwt(user.UserId, claims, TimeSpan.FromMinutes(15));
            var refreshToken = Helpers.RandomString(48);
            tokenService.RefreshTokens[refreshToken] = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.UserId,
                ClientId = clientId,
                Expiry = DateTime.UtcNow.AddDays(30)
            };

            return Results.Json(new
            {
                access_token = accessToken,
                token_type = "Bearer",
                expires_in = 900,
                refresh_token = refreshToken
            });
        }

        if (grantType == "refresh_token")
        {
            var refresh = form["refresh_token"].ToString();
            if (!tokenService.RefreshTokens.TryGetValue(refresh, out var rinfo))
                return Results.BadRequest(new { error = "invalid_grant" });
            if (rinfo.Expiry < DateTime.UtcNow)
            {
                tokenService.RefreshTokens.Remove(refresh);
                return Results.BadRequest(new { error = "invalid_grant", error_description = "refresh expired" });
            }

            var user = users.GetAll().First(u => u.UserId == rinfo.UserId);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId),
                new("name", user.Name ?? user.UserId)
            };

            var accessToken = tokenService.CreateJwt(user.UserId, claims, TimeSpan.FromMinutes(15));
            return Results.Json(new
            {
                access_token = accessToken,
                token_type = "Bearer",
                expires_in = 900
            });
        }

        return Results.BadRequest(new { error = "unsupported_grant_type" });
    }
}