using Application.Contract.DTO;

namespace Application.Services.Entities.TimerSettings;

public interface ITimerSettingsRequestsService
{
    Task<TimerSettingsDto?> Get();
}