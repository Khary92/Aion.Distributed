using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Verifiers.Common.Factories;

public interface IVerifierFactory
{
    IVerifier Create(SortingType sortingType, UseCaseMeta useCaseMeta, Guid traceId);
}