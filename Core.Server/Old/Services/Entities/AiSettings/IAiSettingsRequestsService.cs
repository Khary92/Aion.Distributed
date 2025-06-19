using Proto.DTO.AiSettings;

namespace Service.Server.Old.Services.Entities.AiSettings;

public interface IAiSettingsRequestsService
{
    Task<AiSettingsProto?> Get();
}