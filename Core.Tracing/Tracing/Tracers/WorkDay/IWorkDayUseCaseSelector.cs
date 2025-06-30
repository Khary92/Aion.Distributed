using Core.Server.Tracing.Tracing.Tracers.WorkDay.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.WorkDay;

public interface IWorkDayUseCaseSelector
{
    ICreateWorkDayTraceCollector Create { get; }
}