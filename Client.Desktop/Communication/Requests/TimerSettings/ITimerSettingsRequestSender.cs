using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.TimerSettings;

public interface ITimerSettingsRequestSender
{
    Task<TimerSettingsDto> GetTimerSettings();
    Task<bool> IsTimerSettingExisting();
}