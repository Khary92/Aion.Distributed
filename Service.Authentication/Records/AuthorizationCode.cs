namespace Service.Authorization.Records;

public record AuthorizationCode
{
    public string Code { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string RedirectUri { get; init; } = default!;
    public string UserId { get; init; } = default!;
    public string CodeChallenge { get; init; } = default!;
    public string CodeChallengeMethod { get; init; } = default!;
    public string[] Scopes { get; init; } = Array.Empty<string>();
    public DateTime Expiry { get; init; }
}