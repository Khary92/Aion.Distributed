using Client.Desktop.Communication.RequiresChange.Tracers.WorkDay.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.WorkDay;

public interface IWorkDayUseCaseSelector
{
    ICreateWorkDayTraceCollector Create { get; }
}