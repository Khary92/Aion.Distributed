using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.AiSettings;

public interface IAiSettingsRequestSender
{
    Task<AiSettingsDto?> Get(string aiSettingsId);
}