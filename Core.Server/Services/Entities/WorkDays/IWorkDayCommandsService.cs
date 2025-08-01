using Core.Server.Communication.Records.Commands.Entities.WorkDays;

namespace Core.Server.Services.Entities.WorkDays;

public interface IWorkDayCommandsService
{
    Task Create(CreateWorkDayCommand command);
}