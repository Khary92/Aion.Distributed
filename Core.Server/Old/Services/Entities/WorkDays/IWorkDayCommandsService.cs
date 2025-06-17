using Application.Contract.CQRS.Commands.Entities.WorkDays;

namespace Application.Services.Entities.WorkDays;

public interface IWorkDayCommandsService
{
    Task Create(CreateWorkDayCommand createWorkDayCommand);
}