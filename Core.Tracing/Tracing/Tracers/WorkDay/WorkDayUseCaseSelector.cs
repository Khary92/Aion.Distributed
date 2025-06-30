using Core.Server.Tracing.Tracing.Tracers.WorkDay.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.WorkDay;

public class WorkDayUseCaseSelector(ICreateWorkDayTraceCollector createWorkDayTraceCollector) : IWorkDayUseCaseSelector
{
    public ICreateWorkDayTraceCollector Create => createWorkDayTraceCollector;
}