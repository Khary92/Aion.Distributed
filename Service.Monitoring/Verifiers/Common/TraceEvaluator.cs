using System.Collections.Immutable;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Common;

public class TraceEvaluator(ImmutableList<VerificationStep> verificationSteps)
{
    public Result GetResultState(List<TraceData> traces)
    {
        var immediate = CheckForImmediateResults(traces);
        if (immediate != Result.OngoingValidation) return immediate;

        var counts = traces
            .GroupBy(t => t.LoggingMeta)
            .ToDictionary(g => g.Key, g => g.Count());

        var hasViolation = verificationSteps.Any(step =>
        {
            var found = counts.GetValueOrDefault(step.LoggingMeta, 0);

            if (step.Invoked == Invoked.Optional) return false;
            if (step.Invoked == Invoked.Equals && found == step.Count) return false;
            if (step.Invoked == Invoked.AtLeast && found >= step.Count) return false;

            return true;
        });

        return hasViolation ? Result.Failed : Result.Success;
    }

    private Result CheckForImmediateResults(List<TraceData> traces)
    {
        if (verificationSteps.Count == 0) return Result.NoValidationAvailable;
        if (traces.Any(td => td.LoggingMeta == LoggingMeta.AggregateNotFound)) return Result.EntityNotFound;
        if (traces.Any(td => td.LoggingMeta == LoggingMeta.ExceptionOccured)) return Result.Exception;
        if (traces.Any(td => td.LoggingMeta == LoggingMeta.ActionAborted)) return Result.Aborted;

        return Result.OngoingValidation;
    }
}