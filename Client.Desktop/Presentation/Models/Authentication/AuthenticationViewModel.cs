using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Authentication
{
    public class AuthenticationViewModel : ReactiveObject
    {
        private string _userName = string.Empty;
        private string _password = string.Empty;

        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string AccessToken { get; private set; } = string.Empty;
        public string RefreshToken { get; private set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public AuthenticationViewModel()
        {
            LoginCommand = ReactiveCommand.CreateFromTask(Login);
        }

        private async Task Login()
        {
            // --- PKCE vorbereiten ---
            string RandomString(int len = 32)
            {
                var bytes = RandomNumberGenerator.GetBytes(len);
                return WebEncoders.Base64UrlEncode(bytes);
            }

            var codeVerifier = RandomString(32);

            using var sha = SHA256.Create();
            var challengeBytes = sha.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
            var codeChallenge = WebEncoders.Base64UrlEncode(challengeBytes);

            // --- HttpClient ohne automatische Redirects ---
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };
            using var client = new HttpClient(handler);

            // --- /authorize Request ---
            var authUri = new UriBuilder("https://localhost:5001/authorize");
            var query = new Dictionary<string, string>
            {
                ["response_type"] = "code",
                ["client_id"] = "demo_client",
                ["redirect_uri"] = "http://localhost:5002/callback",
                ["scope"] = "openid api",
                ["state"] = "xyz",
                ["code_challenge"] = codeChallenge,
                ["code_challenge_method"] = "S256",
                ["login_user"] = UserName,
                ["login_pass"] = Password
            };
            authUri.Query = string.Join("&", query.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));

            var response = await client.GetAsync(authUri.Uri);

            // --- Authorization Code aus Redirect extrahieren ---
            string code = null!;
            if (response.Headers.TryGetValues("Location", out var locations))
            {
                var redirectLocation = locations.First();
                code = QueryHelpers.ParseQuery(new Uri(redirectLocation).Query)["code"].ToString();
                Console.WriteLine($"Authorization Code: {code}");
            }
            else
            {
                Console.WriteLine("Kein Location-Header gefunden.");
                return;
            }

            // --- /token Request ---
            var tokenRequest = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = "http://localhost:5002/callback",
                ["client_id"] = "demo_client",
                ["code_verifier"] = codeVerifier
            };

            var tokenResponse = await client.PostAsync(
                "https://localhost:5001/token",
                new FormUrlEncodedContent(tokenRequest)
            );

            var json = await tokenResponse.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<TokenResponse>(json);

            if (tokenData != null)
            {
                AccessToken = tokenData.access_token;
                RefreshToken = tokenData.refresh_token;
                Console.WriteLine($"AccessToken: {AccessToken}");
                Console.WriteLine($"RefreshToken: {RefreshToken}");
            }
        }

        record TokenResponse(string access_token, string token_type, int expires_in, string refresh_token);
    }
}
