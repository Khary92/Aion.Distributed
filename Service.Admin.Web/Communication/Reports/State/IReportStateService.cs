using Service.Admin.Web.Communication.Reports.Records;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Reports.State;

public interface IReportStateService
{
    IReadOnlyList<ReportRecord> Reports { get; }
    SortingType SortingType { get; }
    event Action? OnStateChanged;
    void AddReport(ReportRecord report);
}