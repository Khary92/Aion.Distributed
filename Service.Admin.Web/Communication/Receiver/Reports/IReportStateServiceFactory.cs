using Service.Admin.Web.Services.State;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Receiver.Reports;

public interface IReportStateServiceFactory
{
    IReportStateService? GetService(SortingType sortingType);
}