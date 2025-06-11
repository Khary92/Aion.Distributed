using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.Settings;

namespace Client.Avalonia.Communication.Requests.Settings;

public interface ISettingsRequestSender
{
    Task<SettingsDto> GetSettings();
    Task<bool> IsExportPathValid();
}