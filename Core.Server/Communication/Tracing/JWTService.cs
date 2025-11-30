namespace Core.Server.Communication.Tracing;

public class JwtService
{
    public string Token { get; set; }

    public async Task LoadTokenAsync()
    {
        var tokenFile = "/internal-token/token.jwt";

        if (!File.Exists(tokenFile))
            throw new InvalidOperationException("Internal token not found");

        Token = await File.ReadAllTextAsync(tokenFile);
    }
}