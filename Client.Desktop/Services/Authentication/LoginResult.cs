namespace Client.Desktop.Services.Authentication;

public enum LoginResult
{
    Successful,
    ServiceUnavailable,
    InvalidCredentials,
    RedirectFailed
}