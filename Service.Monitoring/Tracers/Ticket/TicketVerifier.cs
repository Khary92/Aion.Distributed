using System.Collections.Immutable;
using System.Timers;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers;
using Timer = System.Timers.Timer;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketVerifier
{
    private readonly ImmutableList<VerificationStep> _verificationSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandSent, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.AggregateReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.AggregateAdded, Invoked.AtLeast, 1));
    

    private readonly List<TraceData> _traceData = new();
    private readonly Timer _timer = new(1000W);
    
    public TicketVerifier()
    {
        _timer.Elapsed += Elapsed;
        _timer.AutoReset = false;
    }

    public event EventHandler<Report>? VerificationCompleted;

    private void Elapsed(object? sender, ElapsedEventArgs e)
    {
        var result = GetResultState();
        VerificationCompleted?.Invoke(this, new Report(result, _traceData.GetClassTrace()));
    }


    private Result GetResultState()
    {
        var copiedTraceData = _traceData.ToList();

        for (var index = 0; index < _verificationSteps.Count; index++)
        {
            if (copiedTraceData.Count <= index)
                return Result.TimedOut;

            var verificationStep = _verificationSteps[index];
            var traceData = copiedTraceData[index];

            if (traceData.LoggingMeta == LoggingMeta.AggregateNotFound)
                return Result.NotFound;

            if (traceData.LoggingMeta == LoggingMeta.ExceptionOccured)
                return Result.Exception;

            var matchingTraces = copiedTraceData.Count(td => td.LoggingMeta == verificationStep.LoggingMeta);

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

    public void Add(TraceData traceData)
    {
        _traceData.Add(traceData);
        _timer.Stop();
        _timer.Start();
    }
}