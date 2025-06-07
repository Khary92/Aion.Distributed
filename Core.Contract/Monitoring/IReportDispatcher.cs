using Contract.DTO;

namespace Contract.Monitoring;

public interface IReportDispatcher
{
    Task Send(TraceReportDto report);
}