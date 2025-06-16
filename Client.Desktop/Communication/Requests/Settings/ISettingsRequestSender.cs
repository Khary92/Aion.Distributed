using System.Threading.Tasks;
using Client.Desktop.DTO;
using Proto.Requests.Settings;

namespace Client.Desktop.Communication.Requests.Settings;

public interface ISettingsRequestSender
{
    Task<SettingsDto> Send(GetSettingsRequestProto request);
    Task<bool> Send(IsExportPathValidRequestProto request);
    Task<bool> Send(SettingsExistsRequestProto request);
}