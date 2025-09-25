using Core.Server.Tracing.Tracing.Tracers.Statistics.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Statistics;

public interface IStatisticsDataUseCaseSelector
{
    IChangeProductivityTraceCollector ChangeProductivity { get; }
    IChangeTagSelectionTraceCollector ChangeTagSelection { get; }
}