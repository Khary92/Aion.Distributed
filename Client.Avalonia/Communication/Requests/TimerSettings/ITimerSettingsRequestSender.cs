using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.TimerSettings;

namespace Client.Avalonia.Communication.Requests.TimerSettings;

public interface ITimerSettingsRequestSender
{
    Task<TimerSettingsDto> GetTimerSettings();
    Task<bool> IsTimerSettingExisting();
}