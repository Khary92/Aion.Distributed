namespace Client.Desktop.Tracing.Tracing.Tracers.Export.UseCase;

public class ExportTraceCollector() : IExportTraceCollector
{
    public void StartUseCase(Type originClassType)
    {
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, Guid.Empty, "toBeReplaced", originClassType,
        //    "Export requested");
    }

    public void PathSettingsInvalid(Type originClassType, object command)
    {
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.InvalidSettings, Guid.Empty, "toBeReplaced", originClassType,
        //    "PathSettingsInvalid!");
    }

    public void ExportSuccessful(Type originClassType)
    {
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionCompleted, Guid.Empty, "toBeReplaced", originClassType,
        //    "Export successful");
    }

    public void ExceptionOccured(Type originClassType, Exception exception)
    {
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ExceptionOccured, Guid.Empty, "toBeReplaced", originClassType,
        //    "Exception occured: " + exception.StackTrace);
    }
}