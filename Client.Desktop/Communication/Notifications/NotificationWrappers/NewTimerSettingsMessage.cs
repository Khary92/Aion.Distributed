using Client.Desktop.DTO;

namespace Client.Desktop.Communication.Notifications.NotificationWrappers;

public record NewTimerSettingsMessage(TimerSettingsDto TimerSettingsDto);