using System.Text.Json;

namespace Service.Admin.Web.Communication.Authentication;

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
    
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var client = new HttpClient(handler);
        
        var request = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("scope", "openid")
        ]);

        var response = await client.PostAsync(tokenUrl, request);
        Console.WriteLine(response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Raw Response: " + content);

        var payload = JsonDocument.Parse(content);
        Token = payload.RootElement.GetProperty("access_token").GetString();

        Console.WriteLine("Access Token: " + Token);
    }
}