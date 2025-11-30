using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace Client.Desktop.Services.Authentication;

public class TokenService : ITokenService
{
    private readonly HttpClient _client = new(
        new HttpClientHandler
        {
            AllowAutoRedirect = false,
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

    private string _accessToken = "";
    private DateTime _expiry;
    private string _refreshToken = "";

    public event Func<string, Task>? Authenticated;

    public async Task Login(string user, string pass)
    {
        // PKCE minimal
        string RandomStr(int len = 32)
        {
            return WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(len));
        }

        var verifier = RandomStr();
        using var sha = SHA256.Create();
        var challenge = WebEncoders.Base64UrlEncode(
            sha.ComputeHash(Encoding.ASCII.GetBytes(verifier)));


        // 1) /authorize
        var query = new Dictionary<string, string?>
        {
            ["response_type"] = "code",
            ["client_id"] = "demo_client",
            ["redirect_uri"] = "http://localhost:5002/callback",
            ["scope"] = "openid api",
            ["state"] = "xyz",
            ["code_challenge"] = challenge,
            ["code_challenge_method"] = "S256",
            ["login_user"] = user,
            ["login_pass"] = pass
        };

        var uri = QueryHelpers.AddQueryString("https://auth.hiegert.eu/authorize", query);

        var resp = await _client.GetAsync(uri);

        if (resp.Headers.Location == null)
            return;

        var redirect = resp.Headers.Location?.ToString();

        var parsed = QueryHelpers.ParseQuery(new Uri(redirect).Query);
        var code = parsed["code"].ToString();

        // 2) /token
        var tokenReq = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = "http://localhost:5002/callback",
            ["client_id"] = "demo_client",
            ["code_verifier"] = verifier
        };

        var tokenResp = await _client.PostAsync(
            "https://auth.hiegert.eu/token",
            new FormUrlEncodedContent(tokenReq));

        var json = await tokenResp.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<TokenData>(json);

        _accessToken = data!.access_token;
        _refreshToken = data.refresh_token;
        _expiry = DateTime.UtcNow.AddSeconds(data.expires_in);

        if (Authenticated == null) throw new InvalidOperationException("No forwarding receiver set");

        //await startupScheduler.Execute();
        await Authenticated.Invoke(_accessToken);
    }

    public async Task<string> GetToken()
    {
        if (DateTime.UtcNow < _expiry.AddSeconds(-30)) return _accessToken;

        var body = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = _refreshToken
        };

        var resp = await _client.PostAsync(
            "https://localhost:5001/token",
            new FormUrlEncodedContent(body));

        var json = await resp.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<TokenData>(json);

        _accessToken = data.access_token;
        _expiry = DateTime.UtcNow.AddSeconds(data.expires_in);
        if (!string.IsNullOrEmpty(data.refresh_token))
            _refreshToken = data.refresh_token;

        return _accessToken;
    }

    private record TokenData(
        string access_token,
        string token_type,
        int expires_in,
        string refresh_token);
}