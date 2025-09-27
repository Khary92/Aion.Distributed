using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Records.Wrappers;

public record NewTimerSettingsMessage(TimerSettingsWebModel TimerSettings, Guid TraceId);