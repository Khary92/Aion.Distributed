namespace Global.Settings.Types;

public class MonitoringSettings
{
    public string HostName { get; set; } = SettingsDefaultValues.String;
    public int GrpcPort { get; set; } = SettingsDefaultValues.Integer;
    public string DockerHostName { get; set; } = SettingsDefaultValues.String;
    public int DockerGrpcPort { get; set; } = SettingsDefaultValues.Integer;
    public bool IsRunningInDocker { get; set; }
}