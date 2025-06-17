using Application.Contract.DTO;

namespace Application.Services.Entities.Settings;

public interface ISettingsRequestsService
{
    Task<SettingsDto?> Get();
}