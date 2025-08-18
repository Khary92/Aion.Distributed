using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Verifiers.Common.Factories;

public class VerifierFactory(IReportFactory reportFactory) : IVerifierFactory
{
    public IVerifier Create(Guid traceId, SortingType sortingType, UseCaseMeta useCaseMeta)
    {
        return new Verifier(traceId, sortingType, useCaseMeta, reportFactory);
    }
}