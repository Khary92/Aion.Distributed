using System.Timers;
using Service.Monitoring.Shared;
using Service.Monitoring.Verifiers.Common.Records;
using Timer = System.Timers.Timer;

namespace Service.Monitoring.Verifiers.Common;

public class Verifier : IVerifier
{
    private readonly Timer _timer = new(10000);
    private readonly List<TraceData> _traceData = new();
    private readonly UseCaseStateEvaluator _useCaseStateEvaluator;
    private readonly Guid _traceId;

    public Verifier(UseCaseStateEvaluator useCaseStateEvaluator, Guid traceId)
    {
        _useCaseStateEvaluator = useCaseStateEvaluator;
        _traceId = traceId;
        _timer.Elapsed += Elapsed;
        _timer.AutoReset = false;
    }

    public event EventHandler<Report>? VerificationCompleted;

    public void Add(TraceData traceData)
    {
        _traceData.Add(traceData);
        _timer.Stop();
        _timer.Start();
    }

    private void Elapsed(object? sender, ElapsedEventArgs e)
    {
        var report = new Report(_traceData.First().TimeStamp,
            _useCaseStateEvaluator.GetResultState(_traceData),
            _traceData.GetClassTrace(), _traceId);

        VerificationCompleted?.Invoke(this, report);
    }
}