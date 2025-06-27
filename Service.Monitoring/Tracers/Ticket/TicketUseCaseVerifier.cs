using System.Collections.Immutable;
using System.Text;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketUseCaseVerifier()
{
    private const string Alrighty = "Alrighty";

    private readonly ImmutableList<LoggingMeta> _expectedOrder = ImmutableList.Create(
        LoggingMeta.ActionRequested,
        LoggingMeta.ActionCompleted);

    public string Status { get; set; } = Alrighty;
    public string Log { get; set; } = string.Empty;

    private int _index = 0;

    public async Task ProcessAsync(TicketTraceRecord traceRecord)
    {
        if (_expectedOrder[_index] != traceRecord.LoggingMeta)
        {
            Status = "Order wrong";
        }

        Log += ToString(traceRecord);
        
     }

    private DateTimeOffset? _lastTimestamp;

    private string ToString(TicketTraceRecord traceRecord)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("Class [" + traceRecord.OriginClassType + "]");

        if (_lastTimestamp.HasValue)
        {
            var delta = traceRecord.TimeStamp - _lastTimestamp.Value;
            stringBuilder.AppendLine("Latency [" + delta.TotalMilliseconds + " ms]");
        }
        else
        {
            stringBuilder.AppendLine("Latency [N/A]");
        }

        stringBuilder.AppendLine("Log [" + traceRecord.Log + "]");

        _lastTimestamp = traceRecord.TimeStamp;

        return stringBuilder.ToString();
    }
}