using System.Collections.Immutable;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Steps;

public class NoteVerificationProvider : IVerificationProvider
{
    public TraceSinkId TraceSinkId => TraceSinkId.Note;
    public ImmutableList<VerificationStep> GetVerificationSteps(UseCaseMeta useCaseMeta)
    {
        return ImmutableList.Create<VerificationStep>();
    }
}