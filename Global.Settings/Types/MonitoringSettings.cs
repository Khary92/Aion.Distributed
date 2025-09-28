namespace Global.Settings.Types;

public class MonitoringSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 0;
    public int DockerPort { get; set; } = 0;
}