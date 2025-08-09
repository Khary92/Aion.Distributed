using Service.Admin.Web.Communication.Reports.Records;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Reports.State;

public class ReportOverviewStateService(SortingType sortingType) : IReportStateService
{
    private readonly List<ReportRecord> _reports = new();

    public IReadOnlyList<ReportRecord> Reports => _reports.AsReadOnly();
    public event Action? OnStateChanged;

    public void AddReport(ReportRecord report)
    {
        _reports.Add(report);
        NotifyStateChanged();
    }

    public SortingType SortingType => sortingType;

    public void ClearReports()
    {
        _reports.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}