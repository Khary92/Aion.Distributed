using Service.Admin.Web.Communication.Reports.State;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Reports;

public class ReportStateServiceFactory(IEnumerable<IReportStateService?> services) : IReportStateServiceFactory
{
    private readonly Dictionary<SortingType, IReportStateService>
        _services = services.ToDictionary(s => s.SortingType);

    public IReportStateService Get(SortingType sortingType)
    {
        return _services[sortingType];
    }
}