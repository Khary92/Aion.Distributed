using Google.Protobuf.WellKnownTypes;
using Proto.Command.TraceData;

namespace Client.Desktop.Tracing.Tracing;

public static class TraceDataExtensions
{
    public static TraceDataCommandProto ToProto(this TraceDataCommand command)
        => new()
        {
            TraceSinkId = command.TraceSinkId.ToString(),
            UseCaseMeta = command.UseCaseMeta.ToString(),
            LoggingMeta = command.LoggingMeta.ToString(),
            OriginClassType = command.OriginClassType.FullName,
            TraceId = command.TraceId.ToString(),
            Log = command.Log,
            TimeStamp = Timestamp.FromDateTimeOffset(command.TimeStamp)
        };
}