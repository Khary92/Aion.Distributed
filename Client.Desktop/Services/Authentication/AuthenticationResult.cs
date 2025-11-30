namespace Client.Desktop.Services.Authentication;

public enum AuthenticationResult
{
    Successful,
    ServiceUnavailable,
    InvalidCredentials,
    RedirectFailed
}