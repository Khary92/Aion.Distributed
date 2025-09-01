using Service.Admin.Web.Communication.Reports.State;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Reports;

public interface IReportStateServiceFactory
{
    IReportStateService? GetService(SortingType sortingType);
}