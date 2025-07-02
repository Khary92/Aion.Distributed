using Client.Tracing.Tracing.Tracers.WorkDay.UseCase;

namespace Client.Tracing.Tracing.Tracers.WorkDay;

public interface IWorkDayUseCaseSelector
{
    ICreateWorkDayTraceCollector Create { get; }
}