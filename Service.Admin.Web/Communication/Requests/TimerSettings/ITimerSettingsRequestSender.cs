using Proto.DTO.TimerSettings;
using Proto.Requests.TimerSettings;

namespace Service.Admin.Web.Communication.Requests.TimerSettings;

public interface ITimerSettingsRequestSender
{
    Task<TimerSettingsProto> Send(GetTimerSettingsRequestProto request);
    Task<bool> Send(IsTimerSettingExistingRequestProto request);
}