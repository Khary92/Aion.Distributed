using Contract.DTO;

namespace Client.Avalonia.Communication.Notifications.TimerSettings;

public record NewTimerSettingsMessage(TimerSettingsDto TimerSettingsDto);