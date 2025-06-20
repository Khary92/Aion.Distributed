
namespace Service.Server.Communication.CQRS.Commands.Entities.TimerSettings;

public record ChangeDocuTimerSaveIntervalCommand(
    Guid TimerSettingsId,
    int DocuTimerSaveInterval);