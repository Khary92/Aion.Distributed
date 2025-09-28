namespace Global.Settings.Types;

public class ServerSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 0;
    public string DockerHostnameForClient { get; set; } = "localhost";
    public int DockerPort { get; set; } = 0;
}