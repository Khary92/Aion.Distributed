using Service.Admin.Web.Communication.Reports;

namespace Service.Admin.Web.Communication;

public class ReportEventHandler(ILogger<ReportEventHandler> logger, ReportEventBridge bridge) : IReportEventHandler
{
    public event EventHandler<ReportRecord>? ReportReceived;
    
    public virtual void OnReportReceived(ReportRecord report)
    {
        logger.LogInformation("Triggering ReportReceived event");
        ReportReceived?.Invoke(this, report);
        bridge.AddReport(report);
    }
}
