using Service.Monitoring.Communication;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketTraceSink(IReportSender reportSender) : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Ticket;

    private readonly Dictionary<Guid, TicketVerifier> _ticketVerifiers = new();

    public void AddTrace(TraceData traceData)
    {
        if (!_ticketVerifiers.TryGetValue(traceData.TraceId, out var verifier))
        {
            var ticketVerifier = new TicketVerifier();
            ticketVerifier.Add(traceData);
            _ticketVerifiers.Add(traceData.TraceId, ticketVerifier);

            ticketVerifier.VerificationCompleted += SaveReport;

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