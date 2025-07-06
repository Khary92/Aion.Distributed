using System.Timers;
using Service.Monitoring.Shared;
using Service.Monitoring.Verifiers.Common.Records;
using Timer = System.Timers.Timer;

namespace Service.Monitoring.Verifiers.Common;

public class Verifier : IVerifier
{
    private readonly UseCaseStateEvaluator _useCaseStateEvaluator;
    private readonly List<TraceData> _traceData = new();
    private readonly Timer _timer = new(10000);

    public event EventHandler<Report>? VerificationCompleted;

    public Verifier(UseCaseStateEvaluator useCaseStateEvaluator)
    {
        _useCaseStateEvaluator = useCaseStateEvaluator;
        _timer.Elapsed += Elapsed;
        _timer.AutoReset = false;
    }
    
    private void Elapsed(object? sender, ElapsedEventArgs e)
    {
        var report = new Report(_traceData.First().TimeStamp,
            _useCaseStateEvaluator.GetResultState(_traceData),
            _traceData.GetClassTrace());

        VerificationCompleted?.Invoke(this, report);
    }

    public void Add(TraceData traceData)
    {
        _traceData.Add(traceData);
        _timer.Stop();
        _timer.Start();
    }
}