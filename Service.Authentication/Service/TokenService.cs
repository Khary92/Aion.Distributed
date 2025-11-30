using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Service.Authorization.Records;

namespace Service.Authorization.Service;

public class TokenService
{
    private readonly SigningCredentials _credentials;

    public Dictionary<string, AuthorizationCode> AuthCodes { get; } = new();

    public Dictionary<string, RefreshToken> RefreshTokens { get; } = new();

    public RsaSecurityKey Key { get; }

    public TokenService(RsaSecurityKey key)
    {
        Key = key;
        _credentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256);
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
        var serviceUser = "service-user";
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, serviceUser),
            new("name", "internal use only")
        };
        
        var tokenDir = "/internal-token";

        if (!Directory.Exists(tokenDir))
            Directory.CreateDirectory(tokenDir);

        foreach (var file in Directory.GetFiles(tokenDir))
        {
            File.Delete(file);
        }

        await File.WriteAllTextAsync(Path.Combine(tokenDir, "token.jwt"), CreateJwt(serviceUser, claims, validFor));
    }
}