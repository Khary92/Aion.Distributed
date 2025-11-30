namespace Service.Authorization.Records;

public record Client(string ClientId, string[] RedirectUris, bool RequirePkce, string[] AllowedScopes);