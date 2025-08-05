using Core.Server.Tracing.Tracing.Tracers.Statistics.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Statistics;

public class StatisticsDataUseCaseSelector(
    ICreateStatisticsDataTraceCollector createStatisticsDataTraceCollector,
    IChangeProductivityTraceCollector changeProductivityTraceCollector,
    IChangeTagSelectionTraceCollector tagSelectionTraceCollector) : IStatisticsDataUseCaseSelector
{
    public ICreateStatisticsDataTraceCollector Create => createStatisticsDataTraceCollector;
    public IChangeProductivityTraceCollector ChangeProductivity => changeProductivityTraceCollector;
    public IChangeTagSelectionTraceCollector ChangeTagSelection => tagSelectionTraceCollector;
}