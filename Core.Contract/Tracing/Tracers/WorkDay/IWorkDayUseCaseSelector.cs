using Contract.Tracing.Tracers.WorkDay.UseCase;

namespace Contract.Tracing.Tracers.WorkDay;

public interface IWorkDayUseCaseSelector
{
    ICreateWorkDayTraceCollector Create { get; }
}