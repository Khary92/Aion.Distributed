using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Common.Factories;

public class ReportFactory(IEnumerable<IVerificationProvider> verificationProviders) : IReportFactory
{
    private readonly Dictionary<SortingType, IVerificationProvider> _verificationProviders =
        verificationProviders.ToDictionary(ts => ts.SortingType);
    
    public Report Create(Guid traceId, SortingType sortingType, UseCaseMeta useCaseMeta, List<TraceData> traceData)
    {
        var verificationSteps = _verificationProviders[sortingType].GetVerificationSteps(useCaseMeta);
        traceData.Sort((a, b) => a.TimeStamp.CompareTo(b.TimeStamp));
        
        return new Report(traceData.First().TimeStamp,
            traceData.First().SortingType,
            traceData.First().UseCaseMeta,
            new UseCaseStateEvaluator(verificationSteps).GetResultState(traceData),
            GetLatencyInMs(traceData),
            traceData,
            traceId);
    }

    private static int GetLatencyInMs(List<TraceData> traceData)
    {
        return traceData.Last().TimeStamp.Subtract(traceData.First().TimeStamp).Milliseconds;
    }
}