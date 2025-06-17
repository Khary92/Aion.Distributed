using Application.Contract.DTO;

namespace Application.Services.Entities.AiSettings;

public interface IAiSettingsRequestsService
{
    Task<AiSettingsDto?> Get();
}