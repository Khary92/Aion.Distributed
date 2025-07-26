namespace Client.Desktop.DataModels.Local;

public static class SettingsMapperExtensions
{
    public static SettingsClientModel ToClientModel(this SettingsDto dto)
    {
        return new SettingsClientModel(dto.ExportPath);
    }

    public static SettingsDto ToDto(this SettingsClientModel clientModel)
    {
        return new SettingsDto(clientModel.ExportPath);
    }
}