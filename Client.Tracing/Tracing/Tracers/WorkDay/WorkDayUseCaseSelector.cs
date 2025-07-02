using Client.Tracing.Tracing.Tracers.WorkDay.UseCase;

namespace Client.Tracing.Tracing.Tracers.WorkDay;

public class WorkDayUseCaseSelector(ICreateWorkDayTraceCollector createWorkDayTraceCollector) : IWorkDayUseCaseSelector
{
    public ICreateWorkDayTraceCollector Create => createWorkDayTraceCollector;
}