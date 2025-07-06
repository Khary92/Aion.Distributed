using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Verifiers.Common.Factories;

public class VerifierFactory(
    IEnumerable<IVerificationProvider> verificationProviders) : IVerifierFactory
{
    private readonly Dictionary<TraceSinkId, IVerificationProvider> _verificationProviders =
        verificationProviders.ToDictionary(ts => ts.TraceSinkId);

    public IVerifier Create(TraceSinkId traceSinkId, UseCaseMeta useCaseMeta)
    {
        var verificationSteps = _verificationProviders[traceSinkId].GetVerificationSteps(useCaseMeta);
        return new Verifier(new UseCaseStateEvaluator(verificationSteps));
    }
}