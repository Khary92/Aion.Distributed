using Google.Protobuf.WellKnownTypes;
using Proto.Command.TraceData;

namespace Client.Desktop.Tracing.Tracing;

public static class TraceDataExtensions
{
    public static TraceDataCommandProto ToProto(this TraceDataCommand command)
        => new()
        {
            TimeStamp = Timestamp.FromDateTimeOffset(command.TimeStamp),
            Log = command.Log,
            LoggingMeta = command.LoggingMeta.ToString(),
            OriginClassType = command.OriginClassType.FullName,
            TraceId = command.TraceId.ToString(),
            UseCaseMeta = command.UseCaseMeta
        };
}