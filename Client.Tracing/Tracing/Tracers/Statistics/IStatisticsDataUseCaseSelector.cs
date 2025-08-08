using Client.Tracing.Tracing.Tracers.Statistics.UseCase;

namespace Client.Tracing.Tracing.Tracers.Statistics;

public interface IStatisticsDataUseCaseSelector
{
    IChangeProductivityTraceCollector ChangeProductivity { get; }
    IChangeTagSelectionTraceCollector ChangeTagSelection { get; }
}