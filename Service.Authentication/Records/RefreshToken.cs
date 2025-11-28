namespace Service.Authorization.Records;

public record RefreshToken
{
    public string Token { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string UserId { get; init; } = default!;
    public DateTime Expiry { get; init; }
}