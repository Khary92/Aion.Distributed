namespace Global.Settings.Types;

public class ServerSettings
{
    public string HostName { get; set; } = SettingsDefaultValues.String;
    public int GrpcPort { get; set; } = SettingsDefaultValues.Integer;
    public int DockerGrpcPort { get; set; } = SettingsDefaultValues.Integer;
    public string DockerHostName { get; set; } = SettingsDefaultValues.String;
    public bool IsRunningInDocker { get; set; }
}