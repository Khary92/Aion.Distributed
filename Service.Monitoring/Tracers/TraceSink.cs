using System.Collections.Concurrent;
using Service.Monitoring.Communication;
using Service.Monitoring.Shared;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Factories;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Tracers;

public class TraceSink(IReportSender reportSender, IVerifierFactory verifierFactory) : ITraceSink
{
    private readonly ConcurrentDictionary<Guid, IVerifier> _ticketVerifiers = new();

    public void AddTrace(TraceData traceData)
    {
        var verifier = _ticketVerifiers.GetOrAdd(traceData.TraceId, _ =>
        {
            var newVerifier = verifierFactory.Create(traceData.SortingType, traceData.UseCaseMeta, traceData.TraceId);
            newVerifier.VerificationCompleted += SaveReport;
            return newVerifier;
        });

        verifier.Add(traceData);
    }

    private async void SaveReport(object? sender, Report e)
    {
        try
        {
            await reportSender.Send(e);
            _ticketVerifiers.TryRemove(e.TraceId, out _);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}