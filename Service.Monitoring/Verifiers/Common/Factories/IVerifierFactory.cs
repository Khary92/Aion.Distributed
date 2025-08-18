using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Verifiers.Common.Factories;

public interface IVerifierFactory
{
    IVerifier Create(Guid traceId, SortingType sortingType, UseCaseMeta useCaseMeta);
}