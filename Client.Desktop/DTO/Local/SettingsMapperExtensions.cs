namespace Client.Desktop.DTO.Local;

public static class SettingsMapperExtensions
{
    public static SettingsDto ToDto(this SettingsJto jto) => new(jto.ExportPath);
    public static SettingsJto ToJto(this SettingsDto dto) => new(dto.ExportPath);
}