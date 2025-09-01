using System.Timers;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Factories;
using Service.Monitoring.Verifiers.Common.Records;
using Timer = System.Timers.Timer;

namespace Service.Monitoring.Verifiers.Common;

public class Verifier : IVerifier
{
    private readonly Timer _timer = new(10000);
    private readonly List<TraceData> _traceData = [];

    private readonly Guid _traceId;
    private readonly SortingType _sortingType;
    private readonly UseCaseMeta _useCaseMeta;
    private readonly IReportFactory _reportFactory;

    public Verifier(Guid traceId, SortingType sortingType, UseCaseMeta useCaseMeta, IReportFactory reportFactory)
    {
        _traceId = traceId;
        _sortingType = sortingType;
        _useCaseMeta = useCaseMeta;
        _reportFactory = reportFactory;
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
        VerificationCompleted?.Invoke(this, _reportFactory.Create(_traceId, _sortingType, _useCaseMeta, _traceData));
    }
}