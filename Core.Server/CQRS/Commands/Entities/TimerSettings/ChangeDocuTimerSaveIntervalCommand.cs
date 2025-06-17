
namespace Application.Contract.CQRS.Commands.Entities.TimerSettings;

public record ChangeDocuTimerSaveIntervalCommand(
    Guid TimerSettingsId,
    int DocuTimerSaveInterval);