using Client.Desktop.Tracing.Tracing.Tracers.WorkDay.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.WorkDay;

public interface IWorkDayUseCaseSelector
{
    ICreateWorkDayTraceCollector Create { get; }
}