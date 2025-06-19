using Service.Server.CQRS.Commands.Entities.Settings;

namespace Service.Server.Old.Services.Entities.Settings;

public interface ISettingsCommandsService
{
    Task Update(UpdateSettingsCommand updateSettingsCommand);
    Task Create(CreateSettingsCommand createSettingsCommand);
}