namespace Client.Desktop.DTO.Local;

public static class SettingsMapperExtensions
{
    public static SettingsDto ToDto(this SettingsJto jto)
    {
        return new SettingsDto(jto.ExportPath);
    }

    public static SettingsJto ToJto(this SettingsDto dto)
    {
        return new SettingsJto(dto.ExportPath);
    }
}