using Grpc.Core;
using Proto.Command.TraceData;
using Service.Monitoring.Shared;
using Service.Monitoring.Sink;

namespace Service.Monitoring.Communication;

public class TraceDataCommandReceiver(ITraceSink traceSink)
    : TraceDataCommandProtoService.TraceDataCommandProtoServiceBase
{
    public override Task<CommandResponse> SendTraceData(TraceDataCommandProto traceData,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[{traceData.UseCaseMeta}] Timestamp: {traceData.TimeStamp} ID: {traceData.TraceId}, OriginClass: {traceData.OriginClassType}, LoggingMeta: {traceData.LoggingMeta} Log: {traceData.Log}");
        traceSink.AddTrace(traceData.ToRecord());
        return Task.FromResult(new CommandResponse { Success = true });
    }
}