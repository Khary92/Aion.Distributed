namespace Service.Admin.Web.Communication.Records.Commands;

public record WebChangeDocuTimerSaveIntervalCommand(Guid TimerSettingsId, int DocuTimerSaveInterval, Guid TraceId);