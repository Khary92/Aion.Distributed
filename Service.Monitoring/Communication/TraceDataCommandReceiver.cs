using Client.Desktop.Proto.Tracing.Enums;
using Grpc.Core;
using Proto.Command.TraceData;
using Service.Monitoring.Tracers;

namespace Service.Monitoring.Communication;

public class TraceDataCommandReceiver(IEnumerable<ITraceSink> traceSinks)
    : TraceDataCommandProtoService.TraceDataCommandProtoServiceBase
{
    private readonly Dictionary<TraceSinkId, ITraceSink> _sinks = traceSinks.ToDictionary(ts => ts.TraceSinkId);

    public override Task<CommandResponse> SendTraceData(TraceDataCommandProto traceData,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[{traceData.UseCaseMeta}] Timestamp: {traceData.TimeStamp} ID: {traceData.TraceId}, OriginClass: {traceData.OriginClassType}, LoggingMeta: {traceData.LoggingMeta} Log: {traceData.Log}");

        if (!Enum.TryParse(traceData.TraceSinkId, out TraceSinkId traceSinkId))
        {
            Console.WriteLine("No TraceSinkId found");

            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid TraceSinkId"));
        }

        _sinks[traceSinkId].AddTrace(traceData);
        return Task.FromResult<CommandResponse>(new CommandResponse { Success = true });
    }
}