using System.Collections.Immutable;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Common;

public interface IVerificationProvider
{
    SortingType SortingType { get; }
    ImmutableList<VerificationStep> GetVerificationSteps(UseCaseMeta useCaseMeta);
}