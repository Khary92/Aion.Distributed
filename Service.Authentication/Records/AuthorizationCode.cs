namespace Service.Authorization.Records;

public record AuthorizationCode(
    string Code,
    string ClientId,
    string RedirectUri,
    string UserId,
    string CodeChallenge,
    string CodeChallengeMethod,
    string[] Scopes,
    DateTime Expiry);