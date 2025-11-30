using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Service.Authorization.Persistence;
using Service.Authorization.Records;
using Service.Authorization.Service;
using static Microsoft.AspNetCore.Http.Results;

namespace Service.Authorization.Endpoints;

public class 
    TokenEndpoint(IUserRepository users, TokenService tokenService)
{
    public async Task<IResult> Handle(HttpContext httpContext)
    {
        if (!httpContext.Request.HasFormContentType)
            return BadRequest(new { error = "invalid_request" });

        var form = await httpContext.Request.ReadFormAsync();
        var grantType = form["grant_type"].ToString();

        if (grantType == "authorization_code")
        {
            var code = form["code"].ToString();
            var redirectUri = form["redirect_uri"].ToString();
            var clientId = form["client_id"].ToString();
            var codeVerifier = form["code_verifier"].ToString();

            if (!tokenService.AuthCodes.TryGetValue(code, out var stored))
                return BadRequest(new { error = "invalid_grant" });

            if (stored.Expiry < DateTime.UtcNow)
            {
                tokenService.AuthCodes.Remove(code);
                return BadRequest(new { error = "invalid_grant" });
            }

            if (stored.ClientId != clientId || stored.RedirectUri != redirectUri)
                return BadRequest(new { error = "invalid_grant" });

            if (!Helpers.Helpers.VerifyPkce(stored.CodeChallenge, stored.CodeChallengeMethod, codeVerifier))
                return BadRequest(new { error = "invalid_grant", error_description = "pkce_failed" });

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
            var refreshToken = Helpers.Helpers.RandomString(48);
            tokenService.RefreshTokens[refreshToken] = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.UserId,
                ClientId = clientId,
                Expiry = DateTime.UtcNow.AddDays(30)
            };

            return Json(new
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
                return BadRequest(new { error = "invalid_grant" });

            if (rinfo.Expiry < DateTime.UtcNow)
            {
                tokenService.RefreshTokens.Remove(refresh);
                return BadRequest(new { error = "invalid_grant", error_description = "refresh expired" });
            }

            var user = users.GetAll().First(u => u.UserId == rinfo.UserId);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId),
                new("name", user.Name ?? user.UserId)
            };

            var accessToken = tokenService.CreateJwt(user.UserId, claims, TimeSpan.FromMinutes(15));

            return Json(new
            {
                access_token = accessToken,
                token_type = "Bearer",
                expires_in = 900,
                refresh_token = refresh
            });
        }

        return BadRequest(new { error = "unsupported_grant_type" });
    }
}
