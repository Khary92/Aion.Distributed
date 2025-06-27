using Client.Desktop.Tracing.Communication.Tracing;
using Client.Desktop.Tracing.Tracing.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.Export.UseCase;

public class ExportTraceCollector(ITracingDataCommandSender commandSender) : IExportTraceCollector
{
    public async Task StartUseCase(Type originClassType)
    {
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, Guid.Empty,
            "toBeReplaced", originClassType, "Export requested"));
    }

    public async Task PathSettingsInvalid(Type originClassType, object command)
    {
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.InvalidSettings, Guid.Empty,
            "toBeReplaced", originClassType,
            "PathSettingsInvalid!"));
    }

    public async Task ExportSuccessful(Type originClassType)
    {
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionCompleted, Guid.Empty,
            "toBeReplaced", originClassType, "Export successful"));
    }

    public async Task ExceptionOccured(Type originClassType, Exception exception)
    {
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ExceptionOccured, Guid.Empty,
            "toBeReplaced", originClassType, "Exception occured: " + exception.StackTrace));
    }
}