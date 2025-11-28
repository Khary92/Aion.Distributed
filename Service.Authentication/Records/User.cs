namespace Service.Authorization.Records;

public record User(string UserId, string Password, string? Name = null);