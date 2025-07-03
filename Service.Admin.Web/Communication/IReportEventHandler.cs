namespace Service.Admin.Web.Communication;

public interface IReportEventHandler
{
    event EventHandler<ReportRecord> ReportReceived;
    void OnReportReceived(ReportRecord report);
}