using Google.Protobuf.WellKnownTypes;
using Proto.Command.TraceData;
using Service.Monitoring.Shared.Enums;
using Enum = System.Enum;

namespace Service.Monitoring.Shared;

public static class TraceDataExtensions
{
    public static TraceDataCommandProto ToProto(this ServiceTraceDataCommand command)
    {
        return new TraceDataCommandProto
        {
            TraceSinkId = command.SortingType.ToString(),
            UseCaseMeta = command.UseCaseMeta.ToString(),
            LoggingMeta = command.LoggingMeta.ToString(),
            OriginClassType = command.OriginClassType.FullName,
            TraceId = command.TraceId.ToString(),
            Log = command.Log,
            TimeStamp = Timestamp.FromDateTimeOffset(command.TimeStamp)
        };
    }

    public static TraceData ToRecord(this TraceDataCommandProto command)
    {
        return new TraceData(
            Enum.Parse<SortingType>(command.TraceSinkId),
            Enum.Parse<UseCaseMeta>(command.UseCaseMeta),
            Enum.Parse<LoggingMeta>(command.LoggingMeta),
            command.OriginClassType,
            Guid.Parse(command.TraceId),
            command.Log,
            command.TimeStamp.ToDateTimeOffset());
    }
}