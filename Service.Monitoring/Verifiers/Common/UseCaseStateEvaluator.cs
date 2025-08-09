using System.Collections.Immutable;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Common;

public class UseCaseStateEvaluator(ImmutableList<VerificationStep> verificationSteps)
{
    public Result GetResultState(List<TraceData> traces)
    {
        if (verificationSteps.Count == 0) return Result.NoValidationAvailable;

        for (var index = 0; index < verificationSteps.Count; index++)
        {
            if (traces.Count <= index)
                return Result.TimedOut;

            var verificationStep = verificationSteps[index];
            var traceData = traces[index];

            if (traceData.LoggingMeta == LoggingMeta.AggregateNotFound)
                return Result.NotFound;

            if (traceData.LoggingMeta == LoggingMeta.ExceptionOccured)
                return Result.Exception;

            var matchingTraces = traces.Count(td => td.LoggingMeta == verificationStep.LoggingMeta);

            if (verificationStep.Invoked == Invoked.Equals)
            {
                if (traceData.LoggingMeta != verificationStep.LoggingMeta)
                    return Result.WrongOrder;

                if (matchingTraces != verificationStep.Count)
                    return Result.InvalidInvocationCount;

                continue;
            }

            if (matchingTraces < verificationStep.Count)
                return Result.InvalidInvocationCount;
        }

        return Result.Success;
    }
}