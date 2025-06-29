using Proto.Command.TraceData;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketTraceSink : ITraceSink
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

    private void SaveReport(object? sender, Report e)
    {
        string teee = "2";
    }
}