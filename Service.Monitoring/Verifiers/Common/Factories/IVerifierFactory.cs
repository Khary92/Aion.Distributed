using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Verifiers.Common.Factories;

public interface IVerifierFactory
{
    IVerifier Create(TraceSinkId traceSinkId, UseCaseMeta useCaseMeta, Guid traceId);
}