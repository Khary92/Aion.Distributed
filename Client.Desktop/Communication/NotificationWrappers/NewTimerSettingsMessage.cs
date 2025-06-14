using Contract.DTO;

namespace Client.Desktop.Communication.NotificationWrappers;

public record NewTimerSettingsMessage(TimerSettingsDto TimerSettingsDto);