using Contract.DTO;

namespace Client.Desktop.Communication.Notifications.Settings;

public record NewSettingsMessage(SettingsDto Settings);