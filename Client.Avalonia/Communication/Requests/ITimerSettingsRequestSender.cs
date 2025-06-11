using System.Threading.Tasks;
using Proto.Requests.TimerSettings;

namespace Client.Avalonia.Communication.Requests;

public interface ITimerSettingsRequestSender
{
    Task<TimerSettingsProto> GetTimerSettings();
    Task<bool> IsTimerSettingExisting();
}