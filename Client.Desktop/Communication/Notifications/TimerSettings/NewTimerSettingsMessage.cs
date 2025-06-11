using Contract.DTO;

namespace Client.Desktop.Communication.Notifications.TimerSettings;

public record NewTimerSettingsMessage(TimerSettingsDto TimerSettingsDto);