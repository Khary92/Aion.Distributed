using Google.Protobuf.WellKnownTypes;
using Proto.Command.TraceData;

namespace Client.Desktop.Tracing.Tracing;

public static class TraceDataExtensions
{
    public static TraceDataCommandProto ToProto(this TraceDataCommand command)
        => new()
        {
            TraceId = command.TraceId.ToString(),
            TraceSinkId = "Ticket", //TODO
            UseCaseMeta = command.UseCaseMeta,
            LoggingMeta = command.LoggingMeta.ToString(),
            OriginClassType = command.OriginClassType.FullName,
            Log = command.Log,
            TimeStamp = Timestamp.FromDateTimeOffset(command.TimeStamp)
        };
}