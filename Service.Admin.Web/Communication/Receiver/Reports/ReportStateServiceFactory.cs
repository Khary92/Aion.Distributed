using Service.Admin.Web.Services.State;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Receiver.Reports;

public class ReportStateServiceFactory(IEnumerable<IReportStateService?> services) : IReportStateServiceFactory
{
    private readonly Dictionary<SortingType, IReportStateService>
        _services = services.ToDictionary(s => s!.SortingType)!;

    public IReportStateService GetService(SortingType sortingType)
    {
        return _services[sortingType];
    }
}