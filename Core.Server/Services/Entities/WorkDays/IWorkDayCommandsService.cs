using Service.Server.Communication.CQRS.Commands.Entities.WorkDays;

namespace Service.Server.Services.Entities.WorkDays;

public interface IWorkDayCommandsService
{
    Task Create(CreateWorkDayCommand createWorkDayCommand);
}