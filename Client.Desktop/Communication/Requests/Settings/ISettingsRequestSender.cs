using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.Settings;

public interface ISettingsRequestSender
{
    Task<SettingsDto> GetSettings();
    Task<bool> IsExportPathValid();
    Task<bool> IsSettingsExisting();
}