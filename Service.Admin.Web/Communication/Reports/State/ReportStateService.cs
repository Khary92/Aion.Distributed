using Service.Admin.Web.Communication.Reports.Records;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Reports.State;

public class ReportStateService(SortingType sortingType) : IReportStateService
{
    private readonly List<ReportRecord> _reports = [];

    public IReadOnlyList<ReportRecord> Reports => _reports.ToList().AsReadOnly();

    public event Action? OnStateChanged;

    public void AddReport(ReportRecord report)
    {
        _reports.Insert(0, report);
        NotifyStateChanged();
    }

    public SortingType SortingType => sortingType;

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}