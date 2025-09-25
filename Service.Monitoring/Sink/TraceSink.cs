using System.Collections.Concurrent;
using Service.Monitoring.Communication;
using Service.Monitoring.Shared;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Factories;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Sink;

public class TraceSink(IReportSender reportSender, IVerifierFactory verifierFactory, TraceDataSendPolicy sendPolicy)
    : ITraceSink
{
    private readonly ConcurrentDictionary<Guid, IVerifier> _verifiers = new();

    public void AddTrace(TraceData traceData)
    {
        var verifier = _verifiers.GetOrAdd(traceData.TraceId, _ =>
        {
            var newVerifier = verifierFactory.Create(traceData.TraceId, traceData.SortingType, traceData.UseCaseMeta);
            newVerifier.VerificationCompleted += SaveReport;
            return newVerifier;
        });

        verifier.Add(traceData);
    }

    private async void SaveReport(object? sender, Report e)
    {
        try
        {
            await sendPolicy.Policy.ExecuteAsync(() => reportSender.Send(e));
            _verifiers.TryRemove(e.TraceId, out _);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}