using Application.Contract.CQRS.Commands.Entities.Settings;

namespace Application.Services.Entities.Settings;

public interface ISettingsCommandsService
{
    Task Update(UpdateSettingsCommand updateSettingsCommand);
    Task Create(CreateSettingsCommand createSettingsCommand);
}