namespace Global.Settings.Types;

public class AdminSettings
{
    public string InternalHostName { get; set; } = SettingsDefaultValues.String;
    public int SecureExternalGrpcPort { get; set; } = SettingsDefaultValues.Integer;
    public int InternalGrpcPort { get; set; } = SettingsDefaultValues.Integer;
    public int ExposedWebPort { get; set; } = SettingsDefaultValues.Integer;
}