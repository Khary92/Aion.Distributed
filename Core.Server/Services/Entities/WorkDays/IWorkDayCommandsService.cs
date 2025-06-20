using Core.Server.Communication.CQRS.Commands.Entities.WorkDays;

namespace Core.Server.Services.Entities.WorkDays;

public interface IWorkDayCommandsService
{
    Task Create(CreateWorkDayCommand createWorkDayCommand);
}