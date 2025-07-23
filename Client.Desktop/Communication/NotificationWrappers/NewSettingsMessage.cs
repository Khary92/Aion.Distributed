using Client.Desktop.DTO;
using Client.Desktop.DTO.Local;

namespace Client.Desktop.Communication.NotificationWrappers;

public record NewSettingsMessage(SettingsDto Settings);