using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Verifiers.Common.Factories;

public class VerifierFactory(
    IEnumerable<IVerificationProvider> verificationProviders) : IVerifierFactory
{
    private readonly Dictionary<SortingType, IVerificationProvider> _verificationProviders =
        verificationProviders.ToDictionary(ts => ts.SortingType);

    public IVerifier Create(SortingType sortingType, UseCaseMeta useCaseMeta, Guid traceId)
    {
        var verificationSteps = _verificationProviders[sortingType].GetVerificationSteps(useCaseMeta);
        return new Verifier(new UseCaseStateEvaluator(verificationSteps), traceId);
    }
}