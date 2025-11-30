using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Service.Authorization.Service;

namespace Service.Authorization.Endpoints;

public class UserInfoEndpoint(TokenService tokenService)
{
    public Task<IResult> Handle(HttpContext httpContext)
    {
        var auth = httpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(auth) || !auth.StartsWith("Bearer ")) return Task.FromResult(Results.Unauthorized());

        var token = auth.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:5001",
                ValidateAudience = true,
                ValidAudience = "api",
                IssuerSigningKey = tokenService.Key,
                ValidateLifetime = true
            }, out _);
            var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var name = principal.FindFirst("name")?.Value;
            return Task.FromResult(Results.Json(new { sub, name }));
        }
        catch
        {
            return Task.FromResult(Results.Unauthorized());
        }
    }
}