using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.TimerSettings;

namespace Client.Desktop.Communication.Requests.Timer;

public interface ITimerSettingsRequestSender
{
    Task<TimerSettingsClientModel> Send(GetTimerSettingsRequestProto request);
    Task<bool> Send(IsTimerSettingExistingRequestProto request);
}