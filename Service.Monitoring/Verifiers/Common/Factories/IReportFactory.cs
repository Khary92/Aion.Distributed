using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Common.Factories;

public interface IReportFactory
{
    Report Create(Guid traceId, SortingType sortingType, UseCaseMeta useCaseMeta, List<TraceData> traceData);
}