using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Service.Authorization.Records;

namespace Service.Authorization.Service;

public class TokenService
{
    private readonly RsaSecurityKey _key;
    private readonly SigningCredentials _credentials;

    public Dictionary<string, AuthorizationCode> AuthCodes { get; } = new();

    public Dictionary<string, RefreshToken> RefreshTokens { get; } = new();

    public RsaSecurityKey Key => _key;

    public TokenService(RsaSecurityKey key)
    {
        _key = key;
        _credentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256);
    }

    public string CreateJwt(string user, IEnumerable<Claim> claims, TimeSpan validFor)
    {
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            "http://localhost:5001",
            "api",
            claims,
            now,
            now.Add(validFor),
            _credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public async Task GenerateInternalToken(TimeSpan validFor)
    {
        var now = DateTime.UtcNow;

        var claims = new[]
        {
            new Claim("sub", "internal-service"),
            new Claim("role", "internal")
        };

        var token = new JwtSecurityToken(
            issuer: "internal-auth",
            audience: "internal",
            claims: claims,
            notBefore: now,
            expires: now.Add(validFor),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.RsaSha256)
        );

        var tokenDir = "/internal-token";

        if (!Directory.Exists(tokenDir))
            Directory.CreateDirectory(tokenDir);

        foreach (var file in Directory.GetFiles(tokenDir))
        {
            File.Delete(file);
        }

        await File.WriteAllTextAsync(Path.Combine(tokenDir, "token.jwt"),
            new JwtSecurityTokenHandler().WriteToken(token));
    }
}