using System.Text.Json;

namespace Core.Server.Communication.Tracing;

public class JwtService
{
    public string Token { get; set; }

    public async Task LoadTokenAsync()
    {
        var tokenUrl = Environment.GetEnvironmentVariable("TOKEN_URL")!;
        var clientId = Environment.GetEnvironmentVariable("CLIENT_ID")!;
        var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET")!;

        Console.WriteLine("Token URL: " + tokenUrl);
        Console.WriteLine("Client ID: " + clientId);
        Console.WriteLine("Client Secret: " + clientSecret);
        
        using var client = new HttpClient();

        var request = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("scope", "api")
        });

        var response = await client.PostAsync(tokenUrl, request);
        Console.WriteLine(response.StatusCode);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonDocument>();
        var Token = payload.RootElement.GetProperty("access_token").GetString();

        Console.WriteLine("Access Token: " + Token);
    }
}