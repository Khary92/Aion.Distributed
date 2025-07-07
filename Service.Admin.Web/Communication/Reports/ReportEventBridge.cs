using System.Collections.Concurrent;

namespace Service.Admin.Web.Communication;

public class ReportEventBridge
{
    private readonly ConcurrentQueue<ReportRecord> _reports = new();
    public IReadOnlyCollection<ReportRecord> Reports => _reports.ToArray();
    
    public event Action<ReportRecord>? OnNewReport;
    
    public void AddReport(ReportRecord report)
    {
        _reports.Enqueue(report);
        OnNewReport?.Invoke(report);
    }
}