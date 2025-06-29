using Client.Desktop.Proto.Tracing.Enums;
using Client.Desktop.Tracing.Communication.Tracing;

namespace Client.Desktop.Tracing.Tracing.Tracers.Export.UseCase;

public class ExportTraceCollector(ITracingDataCommandSender commandSender) : IExportTraceCollector
{
    public async Task StartUseCase(Type originClassType)
    {
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.Export,
            UseCaseMeta.ExportData,
            LoggingMeta.ActionRequested,
            originClassType,
            Guid.Empty,
            "Export requested",
            DateTimeOffset.Now));
    }

    public async Task PathSettingsInvalid(Type originClassType, object command)
    {
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.Export,
            UseCaseMeta.ExportData,
            LoggingMeta.InvalidSettings,
            originClassType,
            Guid.Empty,
            "Path settings invalid",
            DateTimeOffset.Now));
    }

    public async Task ExportSuccessful(Type originClassType)
    {
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.Export,
            UseCaseMeta.ExportData,
            LoggingMeta.ActionCompleted,
            originClassType,
            Guid.Empty,
            "Export completed",
            DateTimeOffset.Now));
    }

    public async Task ExceptionOccured(Type originClassType, Exception exception)
    {
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.Export,
            UseCaseMeta.ExportData,
            LoggingMeta.ExceptionOccured,
            originClassType,
            Guid.Empty,
            exception.ToString(),
            DateTimeOffset.Now));
    }
}