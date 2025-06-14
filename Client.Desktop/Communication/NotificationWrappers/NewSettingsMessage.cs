using Contract.DTO;

namespace Client.Desktop.Communication.NotificationWrappers;

public record NewSettingsMessage(SettingsDto Settings);