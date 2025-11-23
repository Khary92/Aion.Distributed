// Program.cs
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting();
builder.Services.AddEndpointsApiExplorer();

// In-memory stores (for demo only)
var clients = new List<Client> {
    // Example public client (PKCE) for demo
    new Client {
        ClientId = "demo_client",
        RedirectUris = new[] { "http://localhost:5002/callback" },
        RequirePkce = true,
        AllowedScopes = new[] { "openid", "profile", "api" }
    }
};

var users = new List<User> {
    new User { UserId = "alice", Password = "password", Name = "Alice Example" }
};

var authCodes = new Dictionary<string, AuthorizationCode>(); // code -> info
var refreshTokens = new Dictionary<string, RefreshToken>(); // opaque token -> info

// Generate RSA signing key (persist in production)
using var rsa = RSA.Create(2048);
var rsaKey = new RsaSecurityKey(rsa) { KeyId = Guid.NewGuid().ToString() };
var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);

// Helpers
string CreateJwt(string subject, IEnumerable<Claim> claims, TimeSpan validFor) {
    var now = DateTime.UtcNow;
    var jwt = new JwtSecurityToken(
        issuer: "https://localhost:5001",
        audience: "api",
        claims: claims,
        notBefore: now,
        expires: now.Add(validFor),
        signingCredentials: signingCredentials
    );
    return new JwtSecurityTokenHandler().WriteToken(jwt);
}

string RandomString(int len=32) {
    var b = RandomNumberGenerator.GetBytes(len);
    return WebEncoders.Base64UrlEncode(b);
}

// /authorize endpoint: GET
// Example request:
// GET /authorize?response_type=code&client_id=demo_client&redirect_uri=http%3A%2F%2Flocalhost%3A5002%2Fcallback&scope=openid%20api&state=xyz&code_challenge=...&code_challenge_method=S256
var app = builder.Build();

app.MapGet("/authorize", (HttpRequest req) => {
    var q = req.Query;
    var responseType = q["response_type"].ToString(); // must be "code"
    var clientId = q["client_id"].ToString();
    var redirectUri = q["redirect_uri"].ToString();
    var scope = q["scope"].ToString();
    var state = q["state"].ToString();
    var codeChallenge = q["code_challenge"].ToString();
    var codeChallengeMethod = q["code_challenge_method"].ToString();

    // Basic validation
    var client = clients.FirstOrDefault(c => c.ClientId == clientId);
    if (client == null) return Results.BadRequest("unknown_client");
    if (!client.RedirectUris.Contains(redirectUri)) return Results.BadRequest("invalid_redirect_uri");
    if (responseType != "code") return Results.BadRequest("unsupported_response_type");
    if (client.RequirePkce && string.IsNullOrEmpty(codeChallenge)) return Results.BadRequest("pkce_required");

    // For demo: authenticate user by query (replace by real login UI)
    // e.g. /authorize?...&login_user=alice&login_pass=password
    var loginUser = q["login_user"].ToString();
    var loginPass = q["login_pass"].ToString();
    var user = users.FirstOrDefault(u => u.UserId == loginUser && u.Password == loginPass);
    if (user == null) {
        // return instructions in real app you'd show a login page
        return Results.Ok(new { msg = "Provide login_user & login_pass as query for demo. Example: login_user=alice&login_pass=password" });
    }

    // Create authorization code
    var code = RandomString(32);
    authCodes[code] = new AuthorizationCode {
        Code = code,
        ClientId = clientId,
        RedirectUri = redirectUri,
        UserId = user.UserId,
        CodeChallenge = codeChallenge,
        CodeChallengeMethod = codeChallengeMethod,
        Scopes = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries),
        Expiry = DateTime.UtcNow.AddMinutes(5)
    };

    var redirect = QueryHelpers.AddQueryString(redirectUri, new Dictionary<string,string> {
        ["code"] = code,
        ["state"] = state
    });

    return Results.Redirect(redirect);
});

// /token endpoint: POST
// Accepts application/x-www-form-urlencoded
app.MapPost("/token", async (HttpRequest req) => {
    if (!req.HasFormContentType) return Results.BadRequest("invalid_request");
    var form = await req.ReadFormAsync();
    var grantType = form["grant_type"].ToString();

    if (grantType == "authorization_code") {
        var code = form["code"].ToString();
        var redirectUri = form["redirect_uri"].ToString();
        var clientId = form["client_id"].ToString();
        var codeVerifier = form["code_verifier"].ToString();

        if (!authCodes.TryGetValue(code, out var stored)) return Results.BadRequest(new { error = "invalid_grant" });
        if (stored.Expiry < DateTime.UtcNow) { authCodes.Remove(code); return Results.BadRequest(new { error = "invalid_grant"}); }
        if (stored.ClientId != clientId) return Results.BadRequest(new { error = "invalid_grant" });
        if (stored.RedirectUri != redirectUri) return Results.BadRequest(new { error = "invalid_grant" });

        // Verify PKCE if required
        if (!VerifyPkce(stored.CodeChallenge, stored.CodeChallengeMethod, codeVerifier)) return Results.BadRequest(new { error = "invalid_grant", error_description="pkce_failed" });

        // One-time use
        authCodes.Remove(code);

        // Issue tokens
        var user = users.First(u => u.UserId == stored.UserId);
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
            new Claim("name", user.Name ?? user.UserId)
        };
        var accessToken = CreateJwt(user.UserId, claims, TimeSpan.FromMinutes(15));
        var refreshToken = RandomString(48);
        refreshTokens[refreshToken] = new RefreshToken {
            Token = refreshToken,
            UserId = user.UserId,
            ClientId = clientId,
            Expiry = DateTime.UtcNow.AddDays(30)
        };

        return Results.Json(new {
            access_token = accessToken,
            token_type = "Bearer",
            expires_in = 900,
            refresh_token = refreshToken
        });
    }
    else if (grantType == "refresh_token") {
        var refresh = form["refresh_token"].ToString();
        if (!refreshTokens.TryGetValue(refresh, out var rinfo)) return Results.BadRequest(new { error = "invalid_grant" });
        if (rinfo.Expiry < DateTime.UtcNow) { refreshTokens.Remove(refresh); return Results.BadRequest(new { error = "invalid_grant", error_description="refresh expired" }); }

        var user = users.First(u => u.UserId == rinfo.UserId);
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
            new Claim("name", user.Name ?? user.UserId)
        };
        var accessToken = CreateJwt(user.UserId, claims, TimeSpan.FromMinutes(15));
        return Results.Json(new {
            access_token = accessToken,
            token_type = "Bearer",
            expires_in = 900
        });
    }

    return Results.BadRequest(new { error = "unsupported_grant_type" });
});

// /userinfo endpoint (protected)
app.MapGet("/userinfo", (HttpRequest req) =>
{
    var auth = req.Headers["Authorization"].ToString();
    if (string.IsNullOrEmpty(auth) || !auth.StartsWith("Bearer ")) return Results.Unauthorized();

    var token = auth.Substring("Bearer ".Length).Trim();
    var handler = new JwtSecurityTokenHandler();
    try {
        var principal = handler.ValidateToken(token, new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = "https://localhost:5001",
            ValidateAudience = true,
            ValidAudience = "api",
            IssuerSigningKey = rsaKey,
            ValidateLifetime = true
        }, out _);
        var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var name = principal.FindFirst("name")?.Value;
        return Results.Json(new { sub, name });
    } catch {
        return Results.Unauthorized();
    }
});

app.Run("https://localhost:5001");

// ---------- helper types & functions ----------
bool VerifyPkce(string storedChallenge, string? method, string verifier) {
    if (string.IsNullOrEmpty(storedChallenge)) return false;
    if (method?.ToUpperInvariant() == "S256") {
        using var sha = SHA256.Create();
        var vBytes = Encoding.ASCII.GetBytes(verifier);
        var hash = sha.ComputeHash(vBytes);
        var calc = WebEncoders.Base64UrlEncode(hash);
        return calc == storedChallenge;
    } else {
        // plain
        return verifier == storedChallenge;
    }
}

record Client {
    public string ClientId { get; init; } = default!;
    public string[] RedirectUris { get; init; } = Array.Empty<string>();
    public bool RequirePkce { get; init; } = false;
    public string[] AllowedScopes { get; init; } = Array.Empty<string>();
}

record User {
    public string UserId { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string? Name { get; init; }
}

record AuthorizationCode {
    public string Code { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string RedirectUri { get; init; } = default!;
    public string UserId { get; init; } = default!;
    public string CodeChallenge { get; init; } = default!;
    public string CodeChallengeMethod { get; init; } = default!;
    public string[] Scopes { get; init; } = Array.Empty<string>();
    public DateTime Expiry { get; init; }
}

record RefreshToken {
    public string Token { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string UserId { get; init; } = default!;
    public DateTime Expiry { get; init; }
}
