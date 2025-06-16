using Client.Desktop.DTO;

namespace Client.Desktop.Communication.NotificationWrappers;

public record NewTimerSettingsMessage(TimerSettingsDto TimerSettingsDto);