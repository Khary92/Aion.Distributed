namespace Service.Admin.Web.Communication.Reports.State;

public interface IReportStateService
{
    IReadOnlyList<ReportRecord> Reports { get; }
    event Action? OnStateChanged;
    void AddReport(ReportRecord report);
    void ClearReports();

}