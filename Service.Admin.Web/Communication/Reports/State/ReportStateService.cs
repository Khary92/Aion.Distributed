namespace Service.Admin.Web.Communication.Reports.State;

public class ReportStateService(ILogger<ReportStateService> logger) : IReportStateService
{
    private readonly List<ReportRecord> _reports = new();

    public IReadOnlyList<ReportRecord> Reports => _reports.AsReadOnly();
    public event Action? OnStateChanged;

    public void AddReport(ReportRecord report)
    {
        _reports.Add(report);
        logger.LogInformation("Neuer Bericht hinzugefügt. Aktuelle Anzahl: {Count}", _reports.Count);
        NotifyStateChanged();
    }

    public void ClearReports()
    {
        _reports.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}
