using Service.Monitoring.Communication;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Factories;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketTraceSink(IReportSender reportSender, IVerifierFactory verifierFactory) : ITraceSink
{
    private readonly Dictionary<Guid, IVerifier> _ticketVerifiers = new();
    public TraceSinkId TraceSinkId => TraceSinkId.Ticket;

    public void AddTrace(TraceData traceData)
    {
        if (!_ticketVerifiers.TryGetValue(traceData.TraceId, out var verifier))
        {
            var newVerifier = verifierFactory.Create(traceData.TraceSinkId, traceData.UseCaseMeta);
            _ticketVerifiers.Add(traceData.TraceId, newVerifier);
            newVerifier.Add(traceData);

            newVerifier.VerificationCompleted += SaveReport;

            return;
        }

        verifier.Add(traceData);
    }

    private async void SaveReport(object? sender, Report e)
    {
        try
        {
            await reportSender.Send(e);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}