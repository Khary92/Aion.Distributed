using Client.Desktop.Tracing.Tracing.Tracers.WorkDay.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.WorkDay;

public class WorkDayUseCaseSelector(ICreateWorkDayTraceCollector createWorkDayTraceCollector) : IWorkDayUseCaseSelector
{
    public ICreateWorkDayTraceCollector Create => createWorkDayTraceCollector;
}