namespace Service.Admin.Web.Communication.TimerSettings.Records;

public record WebChangeDocuTimerSaveIntervalCommand(Guid TimerSettingsId, int DocuTimerSaveInterval, Guid TraceId);