using System.Threading.Tasks;
using Client.Desktop.DTO;
using Proto.Requests.AiSettings;

namespace Client.Desktop.Communication.Requests.AiSettings;

public interface IAiSettingsRequestSender
{
    Task<AiSettingsDto> Send(GetAiSettingsRequestProto request);
    Task<bool> Send(AiSettingExistsRequestProto request);
}