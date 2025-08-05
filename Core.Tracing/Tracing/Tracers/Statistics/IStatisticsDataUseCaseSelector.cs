using Core.Server.Tracing.Tracing.Tracers.Statistics.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Statistics;

public interface IStatisticsDataUseCaseSelector
{
    ICreateStatisticsDataTraceCollector Create { get; }
    IChangeProductivityTraceCollector ChangeProductivity { get; }
    IChangeTagSelectionTraceCollector ChangeTagSelection { get; }
}