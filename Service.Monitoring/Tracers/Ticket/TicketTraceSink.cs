using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Ticket;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}