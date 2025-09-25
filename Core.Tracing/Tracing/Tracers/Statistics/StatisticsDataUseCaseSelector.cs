using Core.Server.Tracing.Tracing.Tracers.Statistics.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Statistics;

public class StatisticsDataUseCaseSelector(
    IChangeProductivityTraceCollector changeProductivityTraceCollector,
    IChangeTagSelectionTraceCollector tagSelectionTraceCollector) : IStatisticsDataUseCaseSelector
{
    public IChangeProductivityTraceCollector ChangeProductivity => changeProductivityTraceCollector;
    public IChangeTagSelectionTraceCollector ChangeTagSelection => tagSelectionTraceCollector;
}