using System.Threading.Tasks;
using Client.Desktop.DTO;
using Proto.Requests.TimerSettings;

namespace Client.Desktop.Communication.Requests.TimerSettings;

public interface ITimerSettingsRequestSender
{
    Task<TimerSettingsDto> Send(GetTimerSettingsRequestProto request);
    Task<bool> Send(IsTimerSettingExistingRequestProto request);
}