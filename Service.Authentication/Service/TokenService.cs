using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Service.Authorization.Records;

namespace Service.Authorization.Service;

public class TokenService
{
    private readonly Dictionary<string, AuthorizationCode> _authCodes = new(); // code -> info
    private readonly Dictionary<string, RefreshToken> _refreshTokens = new(); // opaque token -> info

    private readonly RsaSecurityKey _key;
    private readonly SigningCredentials _credentials;

    public Dictionary<string, AuthorizationCode> AuthCodes => _authCodes;
    public Dictionary<string, RefreshToken> RefreshTokens => _refreshTokens;
    public RsaSecurityKey Key => _key;
    
    public TokenService(RsaSecurityKey key)
    {
        _key = key;
        _credentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256);
    }

    public string CreateJwt(string subject, IEnumerable<Claim> claims, TimeSpan validFor)
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

    public void AddAuthCode(string code, AuthorizationCode authCode)
    {
        _authCodes[code] = authCode;
    }

    public AuthorizationCode GetAuthCode(string code)
    {
        return _authCodes[code];
    }

    public void AddRefreshToken(string token, RefreshToken refreshToken)
    {
        _refreshTokens[token] = refreshToken;
    }

    public RefreshToken GetRefreshToken(string token)
    {
        return _refreshTokens[token];
    }
}